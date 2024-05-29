using FlightManagementSystemAPI.Contexts;
using FlightManagementSystemAPI.Exceptions.BookingExceptions;
using FlightManagementSystemAPI.Exceptions.FlightExceptions;
using FlightManagementSystemAPI.Model;
using FlightManagementSystemAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlightManagementUnitTests.RepositoryTests
{
    public class BookingRepositoryTest
    {
        private FlightManagementContext _context;
        private BookingRepository _bookingRepository;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<FlightManagementContext>()
                .UseInMemoryDatabase(databaseName: "dummyDB")
                .Options;
            _context = new FlightManagementContext(options);
            var bookingLoggerMock = new Mock<ILogger<BookingRepository>>();
            _bookingRepository = new BookingRepository(_context, bookingLoggerMock.Object);

        }
        [Test]
        public async Task Add_Success()
        {
            // Arrange
            var newBooking = new Booking
            {
               FlightId=1,
               RouteId=2,
               NoOfPersons=1
            };

            // Act
            var result = await _bookingRepository.Add(newBooking);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(1, result.BookingId);
        }
        [Test]
        public async Task GetByKey_Success()
        {
            // Arrange
            var newBooking = new Booking
            {
                FlightId = 1,
                RouteId = 2,
                NoOfPersons = 1
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
            var nonExistentBookingId = 999; // Assuming invalid booking ID

            // Act & Assert
            Assert.ThrowsAsync<Exception>(() => _bookingRepository.Get(nonExistentBookingId));
        }

        [Test]
        public async Task Update_Success()
        {
            // Arrange
            var newBooking = new Booking
            {
                FlightId = 1,
                RouteId = 2,
                NoOfPersons = 1
            };
            var addedBooking = await _bookingRepository.Add(newBooking);

            // Modify some data in the booking
            addedBooking.NoOfPersons = 2;

            // Act
            var result = await _bookingRepository.Update(addedBooking);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(2, result.NoOfPersons);
        }

        [Test]
        public void Update_Failure_BookingNotFound()
        {
            // Arrange
            var nonExistentBooking = new Booking
            {
                BookingId = 999, // Assuming invalid booking ID
                FlightId = 1,
                RouteId = 2,
                NoOfPersons = 2
            };

            // Act & Assert
            Assert.ThrowsAsync<Exception>(() => _bookingRepository.Update(nonExistentBooking));
        }

        [Test]
        public async Task DeleteByKey_Success()
        {
            // Arrange
            var newBooking = new Booking
            {
                FlightId = 1,
                RouteId = 2,
                NoOfPersons = 1
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
            var nonExistentBookingId = 999; // Assuming invalid booking ID

            // Act & Assert
            Assert.ThrowsAsync<Exception>(() => _bookingRepository.Delete(nonExistentBookingId));
        }

        [Test]
        public async Task GetAll_Success()
        {
            // Arrange
            var newBooking1 = new Booking
            {
                FlightId = 1,
                RouteId = 2,
                NoOfPersons = 1
            };
            var newBooking2 = new Booking
            {
                FlightId = 2,
                RouteId = 3,
                NoOfPersons = 2
            };
            List<Booking> addedBookings = new List<Booking> { newBooking1, newBooking2 };
            foreach (var booking in addedBookings)
            {
                await _bookingRepository.Add(booking);
            }

            // Act
            var result = await _bookingRepository.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(addedBookings.Count, result.Count()); // Assuming only the added bookings are in the database
            foreach (var addedBooking in addedBookings)
            {
                Assert.IsTrue(result.Any(b => b.FlightId == addedBooking.FlightId && b.RouteId == addedBooking.RouteId && b.NoOfPersons == addedBooking.NoOfPersons));
            }
        }

        [Test]
        public void GetAll_Failure_NoBookingsPresent()
        {
            // Arrange
            // Ensure no bookings are added

            // Act & Assert
            Assert.ThrowsAsync<Exception>(() => _bookingRepository.GetAll());
        }
        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
