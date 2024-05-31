using System;
using System.Threading.Tasks;
using FlightManagementSystemAPI.Exceptions.BookingExceptions;
using FlightManagementSystemAPI.Exceptions.UserExceptions;
using FlightManagementSystemAPI.Interfaces;
using FlightManagementSystemAPI.Model;
using FlightManagementSystemAPI.Model.DTOs;
using FlightManagementSystemAPI.Repositories;
using Microsoft.Extensions.Logging;

namespace FlightManagementSystemAPI.Services
{
    public class BookingService : IBookingService
    {
        private readonly IRepository<int, Flight> _flightRepository;
        private readonly IRepository<int, FlightRoute> _routeRepository;
        private readonly IRepository<int, Booking> _bookingRepository;
        private readonly ILogger<BookingService> _logger;

        public BookingService(
            IRepository<int, Flight> flightRepository,
            IRepository<int, FlightRoute> routeRepository,
            IRepository<int, Booking> bookingRepository,
            ILogger<BookingService> logger)
        {
            _flightRepository = flightRepository;
            _routeRepository = routeRepository;
            _bookingRepository = bookingRepository;
            _logger = logger;
        }

        public async Task<ReturnBookingDTO> AddBooking(BookingDTO bookingDTO)
        {
            _logger.LogInformation("AddBooking method called with parameters: {BookingDTO}", bookingDTO);
            try
            {
                // Retrieve flight and route information
                var flight = await _flightRepository.Get(bookingDTO.FlightId);
                var route = await _routeRepository.Get(bookingDTO.RouteId);

                if (flight == null)
                {
                    _logger.LogError("Invalid flight ID: {FlightId}", bookingDTO.FlightId);
                    throw new ArgumentException("Invalid flight ID.");
                }
                if (route == null)
                {
                    _logger.LogError("Invalid route ID: {RouteId}", bookingDTO.RouteId);
                    throw new ArgumentException("Invalid route ID.");
                }

                // Calculate total amount based on the number of persons and route price
                float pricePerPerson = route.PricePerPerson;
                float totalAmount = pricePerPerson * bookingDTO.NoOfPersons;

                // Create a new booking
                var newBooking = new Booking
                {
                    Flight = flight,
                    FlightId = bookingDTO.FlightId,
                    FlightRoute = route,
                    RouteId = bookingDTO.RouteId,
                    NoOfPersons = bookingDTO.NoOfPersons,
                    TotalAmount = totalAmount,
                    Cancellations = new List<Cancellation>(),
                    Payments = new List<Payment>()
                };

                // Save the booking to the repository
                var savedBooking = await _bookingRepository.Add(newBooking);

                // Return the booking details
                var returnBookingDTO = new ReturnBookingDTO
                {
                    BookingId = savedBooking.BookingId,
                    FlightId = savedBooking.FlightId,
                    RouteId = savedBooking.RouteId,
                    NoOfPersons = savedBooking.NoOfPersons,
                    TotalAmount = savedBooking.TotalAmount
                };

                _logger.LogInformation("Booking added successfully: {ReturnBookingDTO}", returnBookingDTO);
                return returnBookingDTO;
            }
            catch (BookingException ex)
            {
                _logger.LogError(ex, "BookingException occurred while adding booking: {BookingDTO}", bookingDTO);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while adding booking: {BookingDTO}", bookingDTO);
                throw new BookingException("Error Occurred While Adding Booking: " + ex.Message, ex);
            }
        }

        public async Task<List<ReturnBookingDTO>> GetAllBookings()
        {
            _logger.LogInformation("GetAllBookings method called");
            try
            {
                var bookings = await _bookingRepository.GetAll();
                List<ReturnBookingDTO> returnBookingDTOs = new List<ReturnBookingDTO>();
                foreach (var booking in bookings)
                {
                    returnBookingDTOs.Add(MapBookingToReturnBookingDTO(booking));
                }
                _logger.LogInformation("All bookings retrieved successfully: {ReturnBookingDTOs}", returnBookingDTOs);
                return returnBookingDTOs;
            }
            catch (BookingException ex)
            {
                _logger.LogError(ex, "BookingException occurred while getting all bookings");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while getting all bookings");
                throw new BookingException("Error Occurred While Getting All the Bookings: " + ex.Message, ex);
            }
        }

        private ReturnBookingDTO MapBookingToReturnBookingDTO(Booking booking)
        {
            _logger.LogInformation("MapBookingToReturnBookingDTO method called with parameters: {Booking}", booking);
            var returnBookingDTO = new ReturnBookingDTO
            {
                BookingId = booking.BookingId,
                FlightId = booking.FlightId,
                RouteId = booking.RouteId,
                NoOfPersons = booking.NoOfPersons,
                TotalAmount = booking.TotalAmount
            };
            _logger.LogInformation("ReturnBookingDTO mapped from Booking: {ReturnBookingDTO}", returnBookingDTO);
            return returnBookingDTO;
        }
    }
}
