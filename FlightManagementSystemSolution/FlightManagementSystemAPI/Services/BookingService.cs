using System;
using System.Threading.Tasks;
using FlightManagementSystemAPI.Exceptions.BookingExceptions;
using FlightManagementSystemAPI.Exceptions.FlightExceptions;
using FlightManagementSystemAPI.Exceptions.RouteExceptions;
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
        private readonly IRepository<int, User> _userRepository;
        private readonly ILogger<BookingService> _logger;

        public BookingService(
            IRepository<int, Flight> flightRepository,
            IRepository<int, FlightRoute> routeRepository,
            IRepository<int, Booking> bookingRepository,
               IRepository<int, User> userRepository,
            ILogger<BookingService> logger)
        {
            _flightRepository = flightRepository;
            _routeRepository = routeRepository;
            _bookingRepository = bookingRepository;
            _userRepository = userRepository;
            _logger = logger;
        }
        #region  Add Booking
        public async Task<ReturnBookingDTO> AddBooking(BookingDTO bookingDTO)
        {
            _logger.LogInformation("AddBooking method called with parameters: {BookingDTO}", bookingDTO);
            try
            {
                // Retrieve flight and route information
                var flight = await _flightRepository.Get(bookingDTO.FlightId);
                var route = await _routeRepository.Get(bookingDTO.RouteId);
                var user = await _userRepository.Get(bookingDTO.UserId);
                if (route.SeatsAvailable == 0)
                {
                    throw new SeatsFilledException("Flight Seats are filled for this route.Try with other flight");
                }
                if (user == null)
                {
                    _logger.LogError("Invalid user ID: {UserId}", bookingDTO.UserId);
                    throw new UserNotFoundException("Invalid user ID.");
                }
                if (flight == null)
                {
                    _logger.LogError("Invalid flight ID: {FlightId}", bookingDTO.FlightId);
                    throw new FlightNotFoundException("Invalid flight ID.");
                }
                if (route == null)
                {
                    _logger.LogError("Invalid route ID: {RouteId}", bookingDTO.RouteId);
                    throw new RouteNotFoundException("Invalid route ID.");
                }
                if (bookingDTO.NoOfPersons == 0)
                {
                    _logger.LogError("Number of persons is zero for booking: {BookingDTO}", bookingDTO);
                    throw new InvalidNumberOfPersonException("Number of persons cannot be zero");
                }

                // Calculate total amount based on the number of persons and route price
                float pricePerPerson = route.PricePerPerson;
                float totalAmount = pricePerPerson * bookingDTO.NoOfPersons;

                // Create a new booking
                var newBooking = new Booking
                {
                    UserId = bookingDTO.UserId,
                    User = user,
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

            catch (SeatsFilledException ex)
            {
                throw;
            }
            catch (InvalidNumberOfPersonException ex)
            {
                _logger.LogError(ex, "Invalid number of persons exception occurred: {BookingDTO}", bookingDTO);
                throw;
            }
            catch (UserNotFoundException ex)
            {
                _logger.LogError(ex, "User not found exception occurred: {BookingDTO}", bookingDTO);
                throw;
            }
            catch (FlightNotFoundException ex)
            {
                _logger.LogError(ex, "Flight not found exception occurred: {BookingDTO}", bookingDTO);
                throw;
            }
            catch (RouteNotFoundException ex)
            {
                _logger.LogError(ex, "Route not found exception occurred: {BookingDTO}", bookingDTO);
                throw;
            }
            catch (BookingException ex)
            {
                _logger.LogError(ex, "Booking exception occurred while adding booking: {BookingDTO}", bookingDTO);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "General exception occurred while adding booking: {BookingDTO}", bookingDTO);
                throw new BookingException("Error occurred while adding booking: " + ex.Message, ex);
            }
        }

        #endregion
        #region GetAllBookings
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
        #endregion
        #region GetAllBookingsByUserId
        public async Task<List<ReturnBookingDTO>> GetAllBookingsByUserId(int userId)
        {
            _logger.LogInformation("GetAllBookings method called for user ID: {UserId}", userId);
            try
            {
                // Assuming GetUserById method retrieves a user by ID
                var user = await _userRepository.Get(userId);
                if (user == null)
                {
                    _logger.LogError("UserNotFoundException occurred for user ID {UserId}", userId);
                    throw new UserNotFoundException("User with ID " + userId + " not found");
                }

                var bookings = await _bookingRepository.GetAll(); 
                List<ReturnBookingDTO> returnBookingDTOs = new List<ReturnBookingDTO>();

                // Filter bookings for the specified user ID
                foreach (var booking in bookings)
                {
                    if (booking.UserId == userId)
                    {
                        returnBookingDTOs.Add(MapBookingToReturnBookingDTO(booking));
                    }
                }

                _logger.LogInformation("Bookings for user ID {UserId} retrieved successfully: {ReturnBookingDTOs}", userId, returnBookingDTOs);
                return returnBookingDTOs;
            }
            catch (BookingException ex)
            {
                _logger.LogError(ex, "BookingException occurred while getting bookings for user ID {UserId}", userId);
                throw;
            }
            catch (UserNotFoundException ex)
            {
                _logger.LogError(ex, "UserNotFoundException occurred for user ID {UserId}", userId);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while getting bookings for user ID {UserId}", userId);
                throw new BookingException("Error occurred while getting bookings for user ID " + userId + ": " + ex.Message, ex);
            }
        }

        #endregion
        #region GetBookingById
        public async Task<ReturnBookingDTO> GetBookingById(int id)
        {
            _logger.LogInformation("GetBookingById method called with id: {Id}", id);
            try
            {
                var booking = await _bookingRepository.Get(id);
                if (booking == null)
                {
                    _logger.LogInformation("No booking found with id: {Id}", id);
                    return null;
                }
                var returnBookingDTO = MapBookingToReturnBookingDTO(booking);
                _logger.LogInformation("Booking retrieved successfully: {ReturnBookingDTO}", returnBookingDTO);
                return returnBookingDTO;
            }
            catch (BookingException ex)
            {
                _logger.LogError(ex, "BookingException occurred while getting booking with id: {Id}", id);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while getting booking with id: {Id}", id);
                throw new BookingException("Error Occurred While Getting the Booking: " + ex.Message, ex);
            }
        }
        #endregion
        #region DeleteBooking
        public async Task<ReturnBookingDTO> DeleteBookingById(int bookingId)
        {
            try
            {
                _logger.LogInformation("Deleting booking...");
                Booking booking = await _bookingRepository.Delete(bookingId);
                if (booking == null)
                {
                    throw new BookingNotFoundException($"No booking with id {bookingId} found");
                }
                ReturnBookingDTO returnBookingDTO = MapBookingToReturnBookingDTO(booking);
                _logger.LogInformation("Booking deleted successfully.");
                return returnBookingDTO;
            }
            catch (BookingNotFoundException ex)
            {
                _logger.LogError(ex, "Booking not found exception caught.");
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting the booking.");
                throw new BookingException("Error occurred while deleting the booking: " + ex.Message, ex);
            }
        }


        #endregion
        #region Mapper methods
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
        #endregion
    }
    }
