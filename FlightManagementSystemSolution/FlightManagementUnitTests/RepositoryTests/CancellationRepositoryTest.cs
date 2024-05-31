using FlightManagementSystemAPI.Contexts;
using FlightManagementSystemAPI.Exceptions.CancellationExceptions;
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
    public class CancellationRepositoryTest
    {
        private FlightManagementContext _context;
        private CancellationRepository _cancellationRepository;
        private Flight _flight;
        private FlightRoute _route;
        private Booking _booking;
        private Payment _payment;

        [SetUp]
        public async Task Setup()
        {
            var options = new DbContextOptionsBuilder<FlightManagementContext>()
                .UseInMemoryDatabase(databaseName: "dummyDB")
                .Options;
            _context = new FlightManagementContext(options);
            var cancellationLoggerMock = new Mock<ILogger<CancellationRepository>>();
            _cancellationRepository = new CancellationRepository(_context, cancellationLoggerMock.Object);

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
            var newCancellation = new Cancellation
            {
                BookingId = _booking.BookingId,
                Reason = "Booked incorrectly",
                PaymentId = _payment.PaymentId
            };

            // Act
            var result = await _cancellationRepository.Add(newCancellation);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(1, result.CancellationId);
        }

        [Test]
        public void Add_Failure_InvalidBookingId()
        {
            // Arrange
            var invalidCancellation = new Cancellation
            {
                BookingId = 444, // Invalid BookingId
                Reason = "Booked incorrectly",
                PaymentId = _payment.PaymentId
            };

            // Act & Assert
            Assert.ThrowsAsync<CancellationException>(() => _cancellationRepository.Add(invalidCancellation));
        }

        [Test]
        public async Task GetByKey_Success()
        {
            // Arrange
            var newCancellation = new Cancellation
            {
                BookingId = _booking.BookingId,
                Reason = "Booked incorrectly",
                PaymentId = _payment.PaymentId
            };
            var addedCancellation = await _cancellationRepository.Add(newCancellation);

            // Act
            var result = await _cancellationRepository.Get(addedCancellation.CancellationId);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(addedCancellation.CancellationId, result.CancellationId);
        }

        [Test]
        public void GetByKey_Failure_NotFoundException()
        {
            // Arrange
            var nonExistentCancellationId = 999;

            // Act & Assert
            Assert.ThrowsAsync<CancellationException>(() => _cancellationRepository.Get(nonExistentCancellationId));
        }

        [Test]
        public async Task Update_Success()
        {
            // Arrange
            var newCancellation = new Cancellation
            {
                BookingId = _booking.BookingId,
                Reason = "Booked incorrectly",
                PaymentId = _payment.PaymentId
            };
            var addedCancellation = await _cancellationRepository.Add(newCancellation);

            // Modify some data in the cancellation
            addedCancellation.Reason = "Changed reason";
            addedCancellation.PaymentId = _payment.PaymentId;

            // Act
            var result = await _cancellationRepository.Update(addedCancellation);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual("Changed reason", result.Reason);
            Assert.AreEqual(_payment.PaymentId, result.PaymentId);
        }

        [Test]
        public void Update_Failure_CancellationNotFound()
        {
            // Arrange
            var nonExistentCancellation = new Cancellation
            {
                CancellationId = 999,
                BookingId = _booking.BookingId,
                Reason = "Booked incorrectly",
                PaymentId = _payment.PaymentId
            };

            // Act & Assert
            Assert.ThrowsAsync<CancellationException>(() => _cancellationRepository.Update(nonExistentCancellation));
        }

        [Test]
        public async Task DeleteByKey_Success()
        {
            // Arrange
            var newCancellation = new Cancellation
            {
                BookingId = _booking.BookingId,
                Reason = "Booked incorrectly",
                PaymentId = _payment.PaymentId
            };
            var addedCancellation = await _cancellationRepository.Add(newCancellation);

            // Act
            var result = await _cancellationRepository.Delete(addedCancellation.CancellationId);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(addedCancellation.CancellationId, result.CancellationId);
        }

        [Test]
        public void DeleteByKey_Failure_CancellationNotFound()
        {
            // Arrange
            var nonExistentCancellationId = 999;

            // Act & Assert
            Assert.ThrowsAsync<CancellationException>(() => _cancellationRepository.Delete(nonExistentCancellationId));
        }

        [Test]
        public async Task GetAll_Success()
        {
            // Arrange
            var newCancellation1 = new Cancellation
            {
                BookingId = _booking.BookingId,
                Reason = "Booked incorrectly",
                PaymentId = _payment.PaymentId
            };
            var newCancellation2 = new Cancellation
            {
                BookingId = _booking.BookingId,
                Reason = "Changed plans",
                PaymentId = _payment.PaymentId
            };
            await _cancellationRepository.Add(newCancellation1);
            await _cancellationRepository.Add(newCancellation2);

            // Act
            var result = await _cancellationRepository.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public async Task GetAll_Failure_NoCancellationsPresent()
        {
            // Act
            var result = await _cancellationRepository.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(0, result.Count());
        }
    }
}
