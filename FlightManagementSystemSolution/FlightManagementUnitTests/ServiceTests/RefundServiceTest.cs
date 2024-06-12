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
using FlightManagementSystemAPI.Exceptions.FlightExceptions;
using FlightManagementSystemAPI.Exceptions.RefundExceptions;

namespace FlightManagementUnitTests.ServiceTests
{
    public class RefundServiceTest
    {
        private FlightManagementContext _context;
        private FlightRepository _flightRepository;
        private RouteRepository _routeRepository;
        private BookingRepository _bookingRepository;
        private PaymentRepository _paymentRepository;
        private PaymentService _paymentService;
        private CancellationRepository _cancellationRepository;
        private RefundRepository _refundRepository;
        private RefundService _refundService;

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

            var refundLoggerMock = new Mock<ILogger<RefundRepository>>();
            _refundRepository = new RefundRepository(_context, refundLoggerMock.Object);

            var refundServiceLoggerMock = new Mock<ILogger<RefundService>>();
            _refundService = new RefundService(_cancellationRepository,_refundRepository,refundServiceLoggerMock.Object);
        }
        [Test]
        public async Task Add_Refund_Success()
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

            var paymentDTO = new PaymentDTO
            {
                BookingId = booking.BookingId,
                PaymentMethod = "upi"
            };
            var newPayment = new Payment
            {
                BookingId = paymentDTO.BookingId,
                PaymentMethod = paymentDTO.PaymentMethod
            };
            var payment = await _paymentRepository.Add(newPayment);
            var cancellationDTO = new CancellationDTO
            {
                BookingId = booking.BookingId,
                PaymentId = payment.PaymentId,
                Reason = "sample"
            };
            var newcancellation = new Cancellation
            { 
               BookingId=cancellationDTO.BookingId,
               PaymentId=cancellationDTO.PaymentId,
               Reason=cancellationDTO.Reason
            };
            var cancellation = await _cancellationRepository.Add(newcancellation);

            var refundDTO = new RefundDTO
            {
                CancellationId = cancellation.CancellationId
            };
            var result = await _refundService.AddRefund(refundDTO);
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.RefundId);
        }

        [Test]
        public async Task Get_Refund_Success()
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

            var addedBooking = new Booking
            {
                FlightId = bookingDTO.FlightId,
                RouteId = bookingDTO.RouteId,
                NoOfPersons = bookingDTO.NoOfPersons
            };

            var booking = await _bookingRepository.Add(addedBooking);

            var paymentDTO = new PaymentDTO
            {
                BookingId = booking.BookingId,
                PaymentMethod = "upi"
            };

            var newPayment = new Payment
            {
                BookingId = paymentDTO.BookingId,
                PaymentMethod = paymentDTO.PaymentMethod
            };

            var payment = await _paymentRepository.Add(newPayment);

            var cancellationDTO = new CancellationDTO
            {
                BookingId = booking.BookingId,
                PaymentId = payment.PaymentId,
                Reason = "sample"
            };

            var newCancellation = new Cancellation
            {
                BookingId = cancellationDTO.BookingId,
                PaymentId = cancellationDTO.PaymentId,
                Reason = cancellationDTO.Reason
            };

            var cancellation = await _cancellationRepository.Add(newCancellation);

            var refundDTO = new RefundDTO
            {
                CancellationId = cancellation.CancellationId
            };

            var addedRefund = await _refundService.AddRefund(refundDTO);

            // Act
            var result = await _refundService.GetRefund(addedRefund.RefundId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(addedRefund.RefundId, result.RefundId);
        }

        
        [Test]
        public async Task Delete_Refund_Success()
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

            var addedBooking = new Booking
            {
                FlightId = bookingDTO.FlightId,
                RouteId = bookingDTO.RouteId,
                NoOfPersons = bookingDTO.NoOfPersons
            };

            var booking = await _bookingRepository.Add(addedBooking);

            var paymentDTO = new PaymentDTO
            {
                BookingId = booking.BookingId,
                PaymentMethod = "upi"
            };

            var newPayment = new Payment
            {
                BookingId = paymentDTO.BookingId,
                PaymentMethod = paymentDTO.PaymentMethod
            };

            var payment = await _paymentRepository.Add(newPayment);

            var cancellationDTO = new CancellationDTO
            {
                BookingId = booking.BookingId,
                PaymentId = payment.PaymentId,
                Reason = "sample"
            };

            var newCancellation = new Cancellation
            {
                BookingId = cancellationDTO.BookingId,
                PaymentId = cancellationDTO.PaymentId,
                Reason = cancellationDTO.Reason
            };

            var cancellation = await _cancellationRepository.Add(newCancellation);

            var refundDTO = new RefundDTO
            {
                CancellationId = cancellation.CancellationId
            };

            var addedRefund = await _refundService.AddRefund(refundDTO);

            // Act
            var result = await _refundService.DeleteRefundById(addedRefund.RefundId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(addedRefund.RefundId, result.RefundId);
        }

        [Test]
        public async Task Get_Refund_Failure()
        {
            // Act & Assert
            Assert.ThrowsAsync<RefundException>(async () => await _refundService.GetRefund(999));
        }



        
        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
