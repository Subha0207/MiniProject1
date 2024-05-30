using FlightManagementSystemAPI.Contexts;
using FlightManagementSystemAPI.Exceptions.BookingExceptions;
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
    public class BookingRepositoryTest
    {
        private FlightManagementContext _context;
        private BookingRepository _bookingRepository;
        private Flight _flight;
        private FlightRoute _route;

        [SetUp]
        public async Task Setup()
        {
            var options = new DbContextOptionsBuilder<FlightManagementContext>()
                .UseInMemoryDatabase(databaseName: "dummyDB")
                .Options;
            _context = new FlightManagementContext(options);
            var bookingLoggerMock = new Mock<ILogger<BookingRepository>>();
            _bookingRepository = new BookingRepository(_context, bookingLoggerMock.Object);

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
            var newBooking = new Booking
            {
                RouteId = _route.RouteId,
                FlightId = _flight.FlightId,
                NoOfPersons = 2,
                TotalAmount = 200.0f
            };

            // Act
            var result = await _bookingRepository.Add(newBooking);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(1, result.BookingId);
        }

        [Test]
        public void Add_Failure_Exception()
        {
            // Arrange
            var invalidBooking = new Booking(); // Missing required fields

            // Act & Assert
            Assert.ThrowsAsync<BookingException>(() => _bookingRepository.Add(invalidBooking));
        }

        [Test]
        public async Task GetByKey_Success()
        {
            // Arrange
            var newBooking = new Booking
            {
                RouteId = _route.RouteId,
                FlightId = _flight.FlightId,
                NoOfPersons = 2,
                TotalAmount = 200.0f
            };
            var addedBooking = await _bookingRepository.Add(newBooking);

            // Act
            var result = await _bookingRepository.Get(addedBooking.BookingId);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(addedBooking.BookingId, result.BookingId);
        }

        [Test]
        public void GetByKey_Failure_NotFoundException()
        {
            // Arrange
            var nonExistentBookingId = 999;

            // Act & Assert
            Assert.ThrowsAsync<BookingException>(() => _bookingRepository.Get(nonExistentBookingId));
        }

        [Test]
        public async Task Update_Success()
        {
            // Arrange
            var newBooking = new Booking
            {
                RouteId = _route.RouteId,
                FlightId = _flight.FlightId,
                NoOfPersons = 2,
                TotalAmount = 200.0f
            };
            var addedBooking = await _bookingRepository.Add(newBooking);

            // Modify some data in the booking
            addedBooking.NoOfPersons = 3;
            addedBooking.TotalAmount = 300.0f;

            // Act
            var result = await _bookingRepository.Update(addedBooking);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(3, result.NoOfPersons);
            Assert.AreEqual(300.0f, result.TotalAmount);
        }

        [Test]
        public void Update_Failure_BookingNotFound()
        {
            // Arrange
            var nonExistentBooking = new Booking
            {
                BookingId = 999,
                RouteId = _route.RouteId,
                FlightId = _flight.FlightId,
                NoOfPersons = 2,
                TotalAmount = 200.0f
            };

            // Act & Assert
            Assert.ThrowsAsync<BookingException>(() => _bookingRepository.Update(nonExistentBooking));
        }

        [Test]
        public async Task DeleteByKey_Success()
        {
            // Arrange
            var newBooking = new Booking
            {
                RouteId = _route.RouteId,
                FlightId = _flight.FlightId,
                NoOfPersons = 2,
                TotalAmount = 200.0f
            };
            var addedBooking = await _bookingRepository.Add(newBooking);

            // Act
            var result = await _bookingRepository.Delete(addedBooking.BookingId);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(addedBooking.BookingId, result.BookingId);
        }

        [Test]
        public void DeleteByKey_Failure_BookingNotFound()
        {
            // Arrange
            var nonExistentBookingId = 999;

            // Act & Assert
            Assert.ThrowsAsync<BookingException>(() => _bookingRepository.Delete(nonExistentBookingId));
        }

        [Test]
        public async Task GetAll_Success()
        {
            // Arrange
            var newBooking1 = new Booking
            {
                RouteId = _route.RouteId,
                FlightId = _flight.FlightId,
                NoOfPersons = 2,
                TotalAmount = 200.0f
            };
            var newBooking2 = new Booking
            {
                RouteId = _route.RouteId,
                FlightId = _flight.FlightId,
                NoOfPersons = 3,
                TotalAmount = 300.0f
            };
            await _bookingRepository.Add(newBooking1);
            await _bookingRepository.Add(newBooking2);

            // Act
            var result = await _bookingRepository.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public void GetAll_Failure_NoBookingsPresent()
        {
            // Arrange

            // Act & Assert
            Assert.ThrowsAsync<BookingException>(() => _bookingRepository.GetAll());
        }
    }
}
