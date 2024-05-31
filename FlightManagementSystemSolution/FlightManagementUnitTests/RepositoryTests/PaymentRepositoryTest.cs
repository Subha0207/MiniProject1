using FlightManagementSystemAPI.Contexts;
using FlightManagementSystemAPI.Exceptions.PaymentExceptions;
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
    public class PaymentRepositoryTest
    {
        private FlightManagementContext _context;
        private PaymentRepository _paymentRepository;
        private Flight _flight;
        private FlightRoute _route;
        private Booking _booking;

        [SetUp]
        public async Task Setup()
        {
            var options = new DbContextOptionsBuilder<FlightManagementContext>()
                .UseInMemoryDatabase(databaseName: "dummyDB")
                .Options;
            _context = new FlightManagementContext(options);
            var paymentLoggerMock = new Mock<ILogger<PaymentRepository>>();
            _paymentRepository = new PaymentRepository(_context, paymentLoggerMock.Object);

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
            var newPayment = new Payment
            {
                BookingId = _booking.BookingId,
                PaymentMethod = "UPI",
                Amount = 200.0f
            };

            // Act
            var result = await _paymentRepository.Add(newPayment);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(1, result.PaymentId);
        }

        [Test]
        public void Add_Failure_InvalidBookingId()
        {
            // Arrange
            var invalidPayment = new Payment
            {
                BookingId = 999, // Invalid BookingId
                PaymentMethod = "UPI",
                Amount = 200.0f
            };

            // Act & Assert
            Assert.ThrowsAsync<PaymentException>(() => _paymentRepository.Add(invalidPayment));
        }



        [Test]
        public async Task GetByKey_Success()
        {
            // Arrange
            var newPayment = new Payment
            {
                BookingId = _booking.BookingId,
                PaymentMethod = "UPI",
                Amount = 200.0f
            };
            var addedPayment = await _paymentRepository.Add(newPayment);

            // Act
            var result = await _paymentRepository.Get(addedPayment.PaymentId);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(addedPayment.PaymentId, result.PaymentId);
        }

        [Test]
        public void GetByKey_Failure_NotFoundException()
        {
            // Arrange
            var nonExistentPaymentId = 999;

            // Act & Assert
            Assert.ThrowsAsync<PaymentException>(() => _paymentRepository.Get(nonExistentPaymentId));
        }

        [Test]
        public async Task Update_Success()
        {
            // Arrange
            var newPayment = new Payment
            {
                BookingId = _booking.BookingId,
                PaymentMethod = "UPI",
                Amount = 200.0f
            };
            var addedPayment = await _paymentRepository.Add(newPayment);

            // Modify some data in the payment
            addedPayment.PaymentMethod = "CreditCard";
            addedPayment.Amount = 250.0f;

            // Act
            var result = await _paymentRepository.Update(addedPayment);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual("CreditCard", result.PaymentMethod);
            Assert.AreEqual(250.0f, result.Amount);
        }

        [Test]
        public void Update_Failure_PaymentNotFound()
        {
            // Arrange
            var nonExistentPayment = new Payment
            {
                PaymentId = 999,
                BookingId = _booking.BookingId,
                PaymentMethod = "UPI",
                Amount = 200.0f
            };

            // Act & Assert
            Assert.ThrowsAsync<PaymentException>(() => _paymentRepository.Update(nonExistentPayment));
        }

        [Test]
        public async Task DeleteByKey_Success()
        {
            // Arrange
            var newPayment = new Payment
            {
                BookingId = _booking.BookingId,
                PaymentMethod = "UPI",
                Amount = 200.0f
            };
            var addedPayment = await _paymentRepository.Add(newPayment);

            // Act
            var result = await _paymentRepository.Delete(addedPayment.PaymentId);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(addedPayment.PaymentId, result.PaymentId);
        }

        [Test]
        public void DeleteByKey_Failure_PaymentNotFound()
        {
            // Arrange
            var nonExistentPaymentId = 999;

            // Act & Assert
            Assert.ThrowsAsync<PaymentException>(() => _paymentRepository.Delete(nonExistentPaymentId));
        }

        [Test]
        public async Task GetAll_Success()
        {
            // Arrange
            var newPayment1 = new Payment
            {
                BookingId = _booking.BookingId,
                PaymentMethod = "UPI",
                Amount = 200.0f
            };
            var newPayment2 = new Payment
            {
                BookingId = _booking.BookingId,
                PaymentMethod = "CreditCard",
                Amount = 250.0f
            };
            await _paymentRepository.Add(newPayment1);
            await _paymentRepository.Add(newPayment2);

            // Act
            var result = await _paymentRepository.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(2, result.Count());
        }
        [Test]
        public async Task GetAll_Failure_NoPaymentsPresent()
        {
            // Act
            var result = await _paymentRepository.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(0, result.Count());
        }
    }
    }
