using FlightManagementSystemAPI.Contexts;
using FlightManagementSystemAPI.Exceptions.RefundExceptions;
using FlightManagementSystemAPI.Model;
using FlightManagementSystemAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace FlightManagementUnitTests.RepositoryTests
{
    public class RefundRepositoryTest
    {
        private FlightManagementContext _context;
        private RefundRepository _refundRepository;
        private Flight _flight;
        private FlightRoute _route;
        private Booking _booking;
        private Payment _payment;
        private Cancellation _cancellation;

        [SetUp]
        public async Task Setup()
        {
            var options = new DbContextOptionsBuilder<FlightManagementContext>()
                .UseInMemoryDatabase(databaseName: "dummyDB")
                .Options;
            _context = new FlightManagementContext(options);
            var refundLoggerMock = new Mock<ILogger<RefundRepository>>();
            _refundRepository = new RefundRepository(_context, refundLoggerMock.Object);

            _flight = new Flight
            {
                FlightName = "ABC Airlines",
                SeatCapacity = 150
            };

            _route = new FlightRoute
            {
                ArrivalLocation = "XYZ",
                DepartureLocation = "ABC",
                NoOfStops = 1,
                PricePerPerson = 100.0f,
                SeatsAvailable = 30,
                DepartureDateTime = DateTime.Now,
                ArrivalDateTime = DateTime.Now.AddHours(2),
                FlightId = _flight.FlightId
            };
            _context.Flights.Add(_flight);
            _context.Routes.Add(_route);
            await _context.SaveChangesAsync();

            _booking = new Booking
            {
                RouteId = _route.RouteId,
                FlightId = _flight.FlightId,
                NoOfPersons = 2,
                TotalAmount = 200.0f
            };
            _context.Bookings.Add(_booking);
            await _context.SaveChangesAsync();

            _payment = new Payment
            {
                BookingId = _booking.BookingId,
                PaymentMethod = "UPI",
                Amount = 200.0f
            };
            _context.Payments.Add(_payment);
            await _context.SaveChangesAsync();

            _cancellation = new Cancellation
            {
                BookingId = _booking.BookingId,
                PaymentId = _payment.PaymentId,
                Reason = "xyz"
            };
            _context.Cancellations.Add(_cancellation);
            await _context.SaveChangesAsync();
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task Add_Success()
        {
            // Arrange
            var newRefund = new Refund
            {
                CancellationId = _cancellation.CancellationId
            };

            // Act
            var result = await _refundRepository.Add(newRefund);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(1, result.RefundId);
        }

        [Test]
        public void Add_Failure_InvalidCancellationId()
        {
            // Arrange
            var invalidRefund = new Refund
            {
                CancellationId = 999 // Invalid CancellationId
            };

            // Act & Assert
            Assert.ThrowsAsync<RefundException>(() => _refundRepository.Add(invalidRefund));
        }

        [Test]
        public async Task GetByKey_Success()
        {
            // Arrange
            var newRefund = new Refund
            {
                CancellationId = _cancellation.CancellationId
            };
            var addedRefund = await _refundRepository.Add(newRefund);

            // Act
            var result = await _refundRepository.Get(addedRefund.RefundId);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(addedRefund.RefundId, result.RefundId);
        }

        [Test]
        public void GetByKey_Failure_NotFoundException()
        {
            // Arrange
            var nonExistentRefundId = 999;

            // Act & Assert
            Assert.ThrowsAsync<RefundException>(() => _refundRepository.Get(nonExistentRefundId));
        }

        [Test]
        public async Task Update_Success()
        {
            // Arrange
            var newRefund = new Refund
            {
                CancellationId = _cancellation.CancellationId
            };
            var addedRefund = await _refundRepository.Add(newRefund);

            // Modify some data in the refund
            addedRefund.RefundStatus = "Approve";

            // Act
            var result = await _refundRepository.Update(addedRefund);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual("Approve", result.RefundStatus);
        }

        [Test]
        public void Update_Failure_RefundNotFound()
        {
            // Arrange
            var nonExistentRefund = new Refund
            {
                RefundId = 999,
                CancellationId = _cancellation.CancellationId,
                RefundStatus = "Approve"
            };

            // Act & Assert
            Assert.ThrowsAsync<RefundException>(() => _refundRepository.Update(nonExistentRefund));
        }

        [Test]
        public async Task DeleteByKey_Success()
        {
            // Arrange
            var newRefund = new Refund
            {
                CancellationId = _cancellation.CancellationId
            };
            var addedRefund = await _refundRepository.Add(newRefund);

            // Act
            var result = await _refundRepository.Delete(addedRefund.RefundId);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(addedRefund.RefundId, result.RefundId);
        }

        [Test]
        public void DeleteByKey_Failure_RefundNotFound()
        {
            // Arrange
            var nonExistentRefundId = 999;

            // Act & Assert
            Assert.ThrowsAsync<RefundException>(() => _refundRepository.Delete(nonExistentRefundId));
        }

        [Test]
        public async Task GetAll_Success()
        {
            // Arrange
            var newRefund1 = new Refund
            {
                CancellationId = _cancellation.CancellationId
            };
            var newRefund2 = new Refund
            {
                CancellationId = _cancellation.CancellationId
            };
            await _refundRepository.Add(newRefund1);
            await _refundRepository.Add(newRefund2);

            // Act
            var result = await _refundRepository.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public async Task GetAll_Failure_NoRefundsPresent()
        {
            // Act & Assert
            Assert.ThrowsAsync<RefundException>(async () => await _refundRepository.GetAll());
        }
    }
}
