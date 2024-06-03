using FlightManagementSystemAPI.Contexts;
using FlightManagementSystemAPI.Interfaces;
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

namespace FlightManagementUnitTests.ServiceTests
{
    public class CancellationServiceTest
    {
        private FlightManagementContext _context;
        private FlightRepository _flightRepository;
        private RouteRepository _routeRepository;
        private BookingRepository _bookingRepository;
        private CancellationRepository _cancellationRepository;
        private CancellationService _cancellationService;
        private PaymentRepository _paymentRepository;

        [SetUp]
        public async Task Setup()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder()
                                .UseInMemoryDatabase("dummyDB");
            _context = new FlightManagementContext(optionsBuilder.Options);

            var flightLoggerMock = new Mock<ILogger<FlightRepository>>();
            _flightRepository = new FlightRepository(_context, flightLoggerMock.Object);

            var routeLoggerMock = new Mock<ILogger<RouteRepository>>();
            _routeRepository = new RouteRepository(_context, routeLoggerMock.Object);
            var bookingLoggerMock = new Mock<ILogger<BookingRepository>>();
            _bookingRepository = new BookingRepository(_context, bookingLoggerMock.Object);
            var paymentLoggerMock = new Mock<ILogger<PaymentRepository>>();
            _paymentRepository = new PaymentRepository(_context, paymentLoggerMock.Object);
            var cancellationLoggerMock = new Mock<ILogger<CancellationRepository>>();
            _cancellationRepository = new CancellationRepository(_context, cancellationLoggerMock.Object);
            var cancellationServiceLoggerMock = new Mock<ILogger<CancellationService>>();
            _cancellationService = new CancellationService(cancellationServiceLoggerMock.Object, _cancellationRepository, _bookingRepository, _paymentRepository, _routeRepository);
        }
        [Test]
        public async Task Add_Cancellation_Success()
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
            var addedBooking = new Booking
            {
                FlightId=bookingDTO.FlightId, RouteId=bookingDTO.RouteId,NoOfPersons=bookingDTO.NoOfPersons

            } ;
            // Act
            var booking = await _bookingRepository.Add(addedBooking);

            //var cancellationDTO = new CancellationDTO
            //{BookingId=booking.BookingId,
            //PaymentId=

            //};
          
            //// Assert
            //Assert.IsNotNull(result);
            //Assert.AreEqual(bookingDTO.FlightId, result.FlightId);
            //Assert.AreEqual(bookingDTO.RouteId, result.RouteId);
            //Assert.AreEqual(bookingDTO.NoOfPersons, result.NoOfPersons);
        }





        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
