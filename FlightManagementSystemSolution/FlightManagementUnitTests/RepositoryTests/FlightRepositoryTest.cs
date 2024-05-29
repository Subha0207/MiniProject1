
using FlightManagementSystemAPI.Contexts;
using FlightManagementSystemAPI.Exceptions.FlightExceptions;
using FlightManagementSystemAPI.Model;
using FlightManagementSystemAPI.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightBookingSystemTest.RepositoryTests
{
    public class FlightRepositoryTests
    {
        private FlightManagementContext _context;
        private FlightRepository _flightRepository;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<FlightManagementContext>()
                .UseInMemoryDatabase(databaseName: "dummyDB")
                .Options;
            _context = new FlightManagementContext(options);
            var flightLoggerMock = new Mock<ILogger<FlightRepository>>();
            _flightRepository = new FlightRepository(_context, flightLoggerMock.Object);

        }


        public async Task Add_Success()
        {
            // Arrange
            var newFlight = new Flight
            {
                FlightName = "Test Airline",
                SeatCapacity = 150
            };

            int initialFlightCount = _context.Flights.Count();

            // Act
            var result = await _flightRepository.Add(newFlight);

            // Assert
            Assert.NotNull(result);
            int expectedFlightId = initialFlightCount + 1;
            Assert.AreEqual(expectedFlightId, result.FlightId);
        }

        [Test]
        public void Add_Failure_Exception()
        {
            // Arrange
            var invalidFlight = new Flight(); // Missing required fields

            // Act & Assert
            Assert.ThrowsAsync<FlightException>(() => _flightRepository.Add(invalidFlight));
        }

        [Test]
        public async Task GetByKey_Success()
        {
            // Arrange
            var newFlight = new Flight
            {
                FlightName = "Test Airline",
                SeatCapacity = 150
            };
            var addedFlight = await _flightRepository.Add(newFlight);

            // Act
            var result = await _flightRepository.Get(addedFlight.FlightId);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(addedFlight.FlightId, result.FlightId);
        }

        [Test]
        public void GetByKey_Failure_NotFoundException()
        {
            // Arrange
            var nonExistentFlightId = 999; // Assuming invalid flight ID

            // Act & Assert
            Assert.ThrowsAsync<FlightException>(() => _flightRepository.Get(nonExistentFlightId));
        }

        [Test]
        public async Task Update_Success()
        {
            // Arrange
            var newFlight = new Flight
            {
                FlightName = "Test Airline",
                SeatCapacity= 150
            };
            var addedFlight = await _flightRepository.Add(newFlight);

            // Modify some data in the flight
            addedFlight.FlightName = "Updated Airline";

            // Act
            var result = await _flightRepository.Update(addedFlight);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual("Updated Airline", result.FlightName);
        }

        [Test]
        public void Update_Failure_FlightNotFound()
        {
            // Arrange
            var nonExistentFlight = new Flight
            {
                FlightId = 999, // Assuming invalid flight ID
                FlightName = "Non-Existent Airline",
                SeatCapacity = 150
            };

            // Act & Assert
            Assert.ThrowsAsync<FlightException>(() => _flightRepository.Update(nonExistentFlight));
        }

        [Test]
        public async Task DeleteByKey_Success()
        {
            // Arrange
            var newFlight = new Flight
            {
                FlightName = "Test Airline",
                SeatCapacity = 150
            };
            var addedFlight = await _flightRepository.Add(newFlight);

            // Act
            var result = await _flightRepository.Delete(addedFlight.FlightId);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(addedFlight.FlightId, result.FlightId);
        }

        [Test]
        public void DeleteByKey_Failure_FlightNotFound()
        {
            // Arrange
            var nonExistentFlightId = 999; // Assuming invalid flight ID

            // Act & Assert
            Assert.ThrowsAsync<FlightException>(() => _flightRepository.Delete(nonExistentFlightId));
        }
        [Test]
        public async Task GetAll_Success()
        {
            // Arrange
            var newFlight1 = new Flight
            {
                FlightName = "Test Airline 1",
                SeatCapacity = 150
            };
            var newFlight2 = new Flight
            {
                FlightName = "Test Airline 2",
                SeatCapacity = 200
            };
            List<Flight> addedFlights = new List<Flight> { newFlight1, newFlight2 };
            foreach (var flight in addedFlights)
            {
                await _flightRepository.Add(flight);
            }

            // Act
            var result = await _flightRepository.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(addedFlights.Count, result.Count()); // Assuming only the added flights are in the database
            foreach (var addedFlight in addedFlights)
            {
                Assert.IsTrue(result.Any(f => f.FlightName == addedFlight.FlightName && f.SeatCapacity == addedFlight.SeatCapacity));
            }
        }

        [Test]
        public void GetAll_Failure_NoFlightsPresent()
        {
            // Arrange
            // Ensure no flights are added

            // Act & Assert
            Assert.ThrowsAsync<FlightException>(() => _flightRepository.GetAll());
        }

        [TearDown]
        public void TearDown()
        {
            _context.Dispose();
        }

    }
}