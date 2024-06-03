using FlightManagementSystemAPI.Contexts;
using FlightManagementSystemAPI.Exceptions.BookingExceptions;
using FlightManagementSystemAPI.Exceptions.FlightExceptions;
using FlightManagementSystemAPI.Exceptions.PaymentExceptions;
using FlightManagementSystemAPI.Interfaces;
using FlightManagementSystemAPI.Model;
using FlightManagementSystemAPI.Model.DTOs;
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
    public class PaymentServiceTest
    {
        private FlightManagementContext _context;
        private FlightRepository _flightRepository;
        private RouteRepository _routeRepository;
        private BookingRepository _bookingRepository;
        private PaymentRepository _paymentRepository;
        private PaymentService _paymentService;

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

            var paymentServiceLoggerMock = new Mock<ILogger<PaymentService>>();

            _paymentService = new PaymentService(paymentServiceLoggerMock.Object,  _paymentRepository, _bookingRepository, _routeRepository);
        }
        [Test]
        public async Task Add_Payment_Success()
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

            var paymentDTO = new PaymentDTO
            {
                BookingId=booking.BookingId,
                PaymentMethod="upi"
            };
            var result = await _paymentService.AddPayment(paymentDTO);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1,result.PaymentId);
          
        }
        [Test]
        public async Task GetAllPaymentSuccess()
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
                FlightId = bookingDTO.FlightId,
                RouteId = bookingDTO.RouteId,
                NoOfPersons = bookingDTO.NoOfPersons

            };
            // Act
            var booking = await _bookingRepository.Add(addedBooking);

            var paymentDTO1 = new PaymentDTO
            {
                BookingId = booking.BookingId,
                PaymentMethod = "upi"
            };
            var paymentDTO2 = new PaymentDTO
            {
                BookingId = booking.BookingId,
                PaymentMethod = "upi"
            };
            await _paymentService.AddPayment(paymentDTO1);
            await _paymentService.AddPayment(paymentDTO2);

            var result = await _paymentService.GetAllPayments();
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count);

        }

        [Test]
        public async Task GetAll_Payments_Failure()
        {
            // Arrange - No payments added

            // Act & Assert
            Assert.ThrowsAsync<PaymentException>(async () => await _paymentService.GetAllPayments());
        }


        [Test]
        public async Task Get_Payment_Success()
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
                FlightId = bookingDTO.FlightId,
                RouteId = bookingDTO.RouteId,
                NoOfPersons = bookingDTO.NoOfPersons

            };
            // Act
            var booking = await _bookingRepository.Add(addedBooking);

            var paymentDTO1 = new PaymentDTO
            {
                BookingId = booking.BookingId,
                PaymentMethod = "upi"
            };
            var paymentDTO2 = new PaymentDTO
            {
                BookingId = booking.BookingId,
                PaymentMethod = "upi"
            };
           var payment= await _paymentService.AddPayment(paymentDTO1);

            var result = await _paymentService.GetPayment(payment.PaymentId);
            // Assert
            Assert.IsNotNull(result);



            Assert.AreEqual(payment.PaymentId, result.PaymentId);
            
        }
        [Test]
        public async Task Get_Payment_Failure()
        {
            // Arrange
            var nonExistentBookingId = 99;

            // Act

            // Assert
            Assert.ThrowsAsync<PaymentException>(async () => await _paymentService.GetPayment(nonExistentBookingId));

        }



        [Test]
        public async Task Delete_Payment_Success()
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
            var booking = new Booking
            {
                FlightId=bookingDTO.FlightId,
                RouteId=bookingDTO.RouteId,
                NoOfPersons=bookingDTO.NoOfPersons
            } ;

            var addedBooking = await _bookingRepository.Add(booking);

            // Act
           
            var paymentDTO1 = new PaymentDTO
            {
                BookingId = booking.BookingId,
                PaymentMethod = "upi"
            };
            
            var payment = await _paymentService.AddPayment(paymentDTO1);
            var result = await _paymentService.DeletePaymentById(payment.PaymentId);
            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(payment.PaymentId, result.PaymentId);
            
        }

        [Test]
        public async Task Delete_Payment_Failure()
        {
            // Arrange

            Assert.ThrowsAsync<PaymentException>(async () => await _paymentService.DeletePaymentById(1));

         
        }


        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
