using FlightManagementSystemAPI.Contexts;
using FlightManagementSystemAPI.Model.DTOs;
using FlightManagementSystemAPI.Model;
using FlightManagementSystemAPI.Repositories;
using FlightManagementSystemAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using FlightManagementSystemAPI.Exceptions.BookingExceptions;
using FlightManagementSystemAPI.Exceptions.FlightExceptions;
using FlightManagementSystemAPI.Interfaces;

namespace FlightManagementUnitTests.ServiceTests
{
    public class BookingServiceTest
    {

        private FlightManagementContext _context;
        private FlightRepository _flightRepository;
        private RouteRepository _routeRepository;
        private BookingService _bookingService;
        private BookingRepository _bookingRepository;
        private UserRepository _userRepository;

        [SetUp]
        public async Task Setup()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder()
                                .UseInMemoryDatabase("dummyDB");
            _context = new FlightManagementContext(optionsBuilder.Options);

            var flightLoggerMock = new Mock<ILogger<FlightRepository>>();
            _flightRepository = new FlightRepository(_context, flightLoggerMock.Object);

            var userLoggerMock = new Mock<ILogger<UserRepository>>();
            _userRepository = new UserRepository(_context, userLoggerMock.Object);
            var routeLoggerMock = new Mock<ILogger<RouteRepository>>();
            _routeRepository = new RouteRepository(_context, routeLoggerMock.Object);
            var bookingLoggerMock = new Mock<ILogger<BookingRepository>>();
            _bookingRepository = new BookingRepository(_context, bookingLoggerMock.Object);
            var bookingServiceLoggerMock = new Mock<ILogger<BookingService>>();

            _bookingService = new BookingService(_flightRepository, _routeRepository, _bookingRepository,_userRepository, bookingServiceLoggerMock.Object);

        }

        [Test]
        public async Task Add_Booking_Success()
        {
            // Arrange
            var flightDTO = new FlightDTO
            {
                FlightName = "abc",
                SeatCapacity = 20
            };

            // Convert DTO to Model
            var addedFlight = new Flight
            {
                FlightName = flightDTO.FlightName,
                SeatCapacity = flightDTO.SeatCapacity
            };

            // Act
            var _flight = await _flightRepository.Add(addedFlight);
            var routeDTO = new RouteDTO
            {
                FlightId = _flight.FlightId,
                ArrivalDateTime = DateTime.Now.AddHours(2),
                ArrivalLocation = "xyz",
                DepartureLocation = "abc",
                NoOfStops = 1,
                PricePerPerson = 5000,
                DepartureDateTime = DateTime.Now,
                SeatsAvailable = 20
            };

            var addedRoute = new FlightRoute
            {
                ArrivalDateTime = routeDTO.ArrivalDateTime,
                ArrivalLocation = routeDTO.ArrivalLocation,
                DepartureDateTime = routeDTO.DepartureDateTime,
                DepartureLocation = routeDTO.DepartureLocation,
                NoOfStops = routeDTO.NoOfStops,
                SeatsAvailable = routeDTO.SeatsAvailable,
                PricePerPerson = routeDTO.PricePerPerson
            };

            // Act
            var _route = await _routeRepository.Add(addedRoute);
            var bookingDTO = new BookingDTO
            {
                FlightId = _flight.FlightId,
                RouteId = _route.RouteId,
                NoOfPersons = 2
            };

            // Act
            var result = await _bookingService.AddBooking(bookingDTO);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(bookingDTO.FlightId, result.FlightId);
            Assert.AreEqual(bookingDTO.RouteId, result.RouteId);
            Assert.AreEqual(bookingDTO.NoOfPersons, result.NoOfPersons);
        }

        [Test]
        public async Task Get_All_Booking_Success()
        {

            // Arrange
            var flightDTO = new FlightDTO
            {
                FlightName = "abc",
                SeatCapacity = 20
            };

            // Convert DTO to Model
            var addedFlight = new Flight
            {
                FlightName = flightDTO.FlightName,
                SeatCapacity = flightDTO.SeatCapacity
            };

            // Act
            var _flight = await _flightRepository.Add(addedFlight);
            var routeDTO = new RouteDTO
            {
                FlightId = _flight.FlightId,
                ArrivalDateTime = DateTime.Now.AddHours(2),
                ArrivalLocation = "xyz",
                DepartureLocation = "abc",
                NoOfStops = 1,
                PricePerPerson = 5000,
                DepartureDateTime = DateTime.Now,
                SeatsAvailable = 20
            };

            var addedRoute = new FlightRoute
            {
                ArrivalDateTime = routeDTO.ArrivalDateTime,
                ArrivalLocation = routeDTO.ArrivalLocation,
                DepartureDateTime = routeDTO.DepartureDateTime,
                DepartureLocation = routeDTO.DepartureLocation,
                NoOfStops = routeDTO.NoOfStops,
                SeatsAvailable = routeDTO.SeatsAvailable,
                PricePerPerson = routeDTO.PricePerPerson
            };

            // Act
            var _route = await _routeRepository.Add(addedRoute);
            var bookingDTO1 = new BookingDTO
            {
                FlightId = _flight.FlightId,
                RouteId = _route.RouteId,
                NoOfPersons = 2
            };
            var bookingDTO2 = new BookingDTO
            {
                FlightId = _flight.FlightId,
                RouteId = _route.RouteId,
                NoOfPersons = 2
            };
            // Act
            await _bookingService.AddBooking(bookingDTO1);
            await _bookingService.AddBooking(bookingDTO2);
            var result = await _bookingService.GetAllBookings();
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);
        }

        [Test]
        public async Task GetAll_Bookings_Failure()
        {
            // Arrange - No bookings added

            // Act & Assert
            Assert.ThrowsAsync<BookingException>(async () => await _bookingService.GetAllBookings());
        }
        [Test]
        public async Task Get_Booking_Success()
        {

            // Arrange
            var flightDTO = new FlightDTO
            {
                FlightName = "abc",
                SeatCapacity = 20
            };

            // Convert DTO to Model
            var addedFlight = new Flight
            {
                FlightName = flightDTO.FlightName,
                SeatCapacity = flightDTO.SeatCapacity
            };

            // Act
            var _flight = await _flightRepository.Add(addedFlight);
            var routeDTO = new RouteDTO
            {
                FlightId = _flight.FlightId,
                ArrivalDateTime = DateTime.Now.AddHours(2),
                ArrivalLocation = "xyz",
                DepartureLocation = "abc",
                NoOfStops = 1,
                PricePerPerson = 5000,
                DepartureDateTime = DateTime.Now,
                SeatsAvailable = 20
            };

            var addedRoute = new FlightRoute
            {
                ArrivalDateTime = routeDTO.ArrivalDateTime,
                ArrivalLocation = routeDTO.ArrivalLocation,
                DepartureDateTime = routeDTO.DepartureDateTime,
                DepartureLocation = routeDTO.DepartureLocation,
                NoOfStops = routeDTO.NoOfStops,
                SeatsAvailable = routeDTO.SeatsAvailable,
                PricePerPerson = routeDTO.PricePerPerson
            };

            // Act
            var _route = await _routeRepository.Add(addedRoute);
            var bookingDTO1 = new BookingDTO
            {
                FlightId = _flight.FlightId,
                RouteId = _route.RouteId,
                NoOfPersons = 2
            };

            // Act
            var booking = await _bookingService.AddBooking(bookingDTO1);
            var result = await _bookingService.GetBookingById(booking.BookingId);
            // Assert
            Assert.IsNotNull(result);



            Assert.AreEqual(booking.FlightId, result.FlightId);
            Assert.AreEqual(booking.RouteId, result.RouteId);
            Assert.AreEqual(booking.NoOfPersons, result.NoOfPersons);

        }
        [Test]
        public async Task Get_Booking_Failure()
        {
            // Arrange
            var nonExistentBookingId = 99;

            // Act

            // Assert
            Assert.ThrowsAsync<BookingException>(async () => await _bookingService.GetBookingById(nonExistentBookingId));

        }




        [Test]
        public async Task Delete_Booking_Success()
        {
            // Arrange
            var flightDTO = new FlightDTO
            {
                FlightName = "abc",
                SeatCapacity = 20
            };

            var addedFlight = new Flight
            {
                FlightName = flightDTO.FlightName,
                SeatCapacity = flightDTO.SeatCapacity
            };

            var _flight = await _flightRepository.Add(addedFlight);
            var routeDTO = new RouteDTO
            {
                FlightId = _flight.FlightId,
                ArrivalDateTime = DateTime.Now.AddHours(2),
                ArrivalLocation = "xyz",
                DepartureLocation = "abc",
                NoOfStops = 1,
                PricePerPerson = 5000,
                DepartureDateTime = DateTime.Now,
                SeatsAvailable = 20
            };

            var addedRoute = new FlightRoute
            {
                ArrivalDateTime = routeDTO.ArrivalDateTime,
                ArrivalLocation = routeDTO.ArrivalLocation,
                DepartureDateTime = routeDTO.DepartureDateTime,
                DepartureLocation = routeDTO.DepartureLocation,
                NoOfStops = routeDTO.NoOfStops,
                SeatsAvailable = routeDTO.SeatsAvailable,
                PricePerPerson = routeDTO.PricePerPerson
            };

            var _route = await _routeRepository.Add(addedRoute);
            var bookingDTO = new BookingDTO
            {
                FlightId = _flight.FlightId,
                RouteId = _route.RouteId,
                NoOfPersons = 2
            };

            var addedBooking = await _bookingService.AddBooking(bookingDTO);

            // Act
            var result = await _bookingService.DeleteBookingById(addedBooking.BookingId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(addedBooking.FlightId, result.FlightId);
            Assert.AreEqual(addedBooking.RouteId, result.RouteId);
            Assert.AreEqual(addedBooking.NoOfPersons, result.NoOfPersons);
        }

        [Test]
        public async Task Delete_Booking_Failure()
        {
            // Arrange

            Assert.ThrowsAsync<BookingException>(async () => await _bookingService.DeleteBookingById(1));


        }






        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
