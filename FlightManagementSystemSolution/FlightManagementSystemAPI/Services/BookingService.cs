

using System;
using System.Threading.Tasks;
using FlightManagementSystemAPI.Exceptions.BookingExceptions;
using FlightManagementSystemAPI.Exceptions.UserExceptions;
using FlightManagementSystemAPI.Interfaces;
using FlightManagementSystemAPI.Model;
using FlightManagementSystemAPI.Model.DTOs;
using FlightManagementSystemAPI.Repositories;

namespace FlightManagementSystemAPI.Services
{
    public class BookingService : IBookingService
    {
        private readonly IRepository<int, Flight> _flightRepository;
        private readonly IRepository<int, FlightRoute> _routeRepository;
        private readonly IRepository<int, Booking> _bookingRepository;
  

        public BookingService(
            IRepository<int, Flight> flightRepository,
            IRepository<int, FlightRoute> routeRepository,
            IRepository<int, Booking> bookingRepository)
        {
            _flightRepository = flightRepository;
            _routeRepository = routeRepository;
            _bookingRepository = bookingRepository;
           
        }

        public async Task<ReturnBookingDTO> AddBooking(BookingDTO bookingDTO)
        {
            // Retrieve flight and route information
            var flight = await _flightRepository.Get(bookingDTO.FlightId);
            var route = await _routeRepository.Get(bookingDTO.RouteId);

            if (flight == null )
            {
                throw new ArgumentException("Invalid flight ID.");
            }
            if (route == null)
            {
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

            return returnBookingDTO;
        }

       
        public async Task<List<ReturnBookingDTO>> GetAllBookings()
        {
            try
            {
                var bookings = await _bookingRepository.GetAll();
                List<ReturnBookingDTO> returnBookingDTOs = new List<ReturnBookingDTO>();
                foreach (var booking in bookings)
                {
                    returnBookingDTOs.Add(MapBookingToReturnBookingDTO(booking));
                }
                return returnBookingDTOs;
            }
            catch (BookingException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new BookingException("Error Occurred While Getting All the Bookings: " + ex.Message, ex);
            }
        }
        private ReturnBookingDTO MapBookingToReturnBookingDTO(Booking booking)
        {
            return new ReturnBookingDTO
            {
                BookingId = booking.BookingId,
                FlightId = booking.FlightId,
                RouteId = booking.RouteId,
                NoOfPersons = booking.NoOfPersons,
                TotalAmount = booking.TotalAmount
            };
        }
    }
}
