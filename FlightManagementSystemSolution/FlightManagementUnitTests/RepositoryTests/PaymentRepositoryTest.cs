using FlightManagementSystemAPI.Contexts;
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

        }


        [Test]
        public async Task Add_Success()
        {
            // Arrange
            var newPayment = new Payment
            {
                BookingId=_booking.BookingId,
                PaymentMethod="UPI",Amount=200.0f
            };

            // Act
            var result = await _paymentRepository.Add(newPayment);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(1, result.PaymentId);
        }
        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

    }
}
