using FlightManagementSystemAPI.Contexts;
using FlightManagementSystemAPI.Exceptions.RouteExceptions;
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

namespace FlightManagementUnitTests.RepositoryTests
{
    public class RouteRepositoryTest
    {
        private FlightManagementContext _context;
        private RouteRepository _routeRepository;
        private FlightRepository _flightRepository;
        private Flight _flight;

        [SetUp]
        public async Task Setup()
        {
            var options = new DbContextOptionsBuilder<FlightManagementContext>()
                .UseInMemoryDatabase(databaseName: "dummyDB")
                .Options;
            _context = new FlightManagementContext(options);
            var routeLoggerMock = new Mock<ILogger<RouteRepository>>();
            _routeRepository = new RouteRepository(_context, routeLoggerMock.Object);
            
            _flight = new Flight
            {
                FlightName = "ABC Airlines",
                SeatCapacity = 150
            };
            _context.Flights.Add(_flight);
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
            var newRoute = new FlightRoute
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

            // Act
            var result = await _routeRepository.Add(newRoute);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(1, result.RouteId);
        }

        [Test]
        public async Task Add_Failure_Exception()
        {
            // Arrange
            var invalidRoute = new FlightRoute(); // Missing required properties

            // Act & Assert
            var ex = Assert.ThrowsAsync<RouteException>(async () =>
            {
                await _routeRepository.Add(invalidRoute);
            });

            Assert.That(ex.InnerException, Is.TypeOf<DbUpdateException>());
        }

        [Test]
        public async Task GetByKey_Success()
        {
            // Arrange
            var newRoute = new FlightRoute
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
            var addedRoute = await _routeRepository.Add(newRoute);

            // Act
            var result = await _routeRepository.Get(addedRoute.RouteId);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(addedRoute.RouteId, result.RouteId);
        }

        [Test]
        public void GetByKey_Failure_NotFoundException()
        {
            // Arrange
            var nonExistentRouteId = 999;

            // Act & Assert
            Assert.ThrowsAsync<RouteException>(() => _routeRepository.Get(nonExistentRouteId));
        }

        [Test]
        public async Task Update_Success()
        {
            // Arrange
            var newRoute = new FlightRoute
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
            var addedRoute = await _routeRepository.Add(newRoute);

            // Modify some data in the route
            addedRoute.SeatsAvailable = 25;

            // Act
            var result = await _routeRepository.Update(addedRoute);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(25, result.SeatsAvailable);
        }

        [Test]
        public void Update_Failure_RouteNotFound()
        {
            // Arrange
            var nonExistentRoute = new FlightRoute
            {
                RouteId = 999,
                ArrivalLocation = "XYZ",
                DepartureLocation = "ABC",
                NoOfStops = 1,
                PricePerPerson = 100.0f,
                SeatsAvailable = 30,
                DepartureDateTime = DateTime.Now,
                ArrivalDateTime = DateTime.Now.AddHours(2),
                FlightId = _flight.FlightId
            };

            // Act & Assert
            Assert.ThrowsAsync<RouteException>(() => _routeRepository.Update(nonExistentRoute));
        }

        [Test]
        public async Task DeleteByKey_Success()
        {
            // Arrange
            var newRoute = new FlightRoute
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
            var addedRoute = await _routeRepository.Add(newRoute);

            // Act
            var result = await _routeRepository.Delete(addedRoute.RouteId);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(addedRoute.RouteId, result.RouteId);
        }

        [Test]
        public void DeleteByKey_Failure_RouteNotFound()
        {
            // Arrange
            var nonExistentRouteId = 999;

            // Act & Assert
            Assert.ThrowsAsync<RouteException>(() => _routeRepository.Delete(nonExistentRouteId));
        }

        [Test]
        public async Task GetAll_Success()
        {
            // Arrange
            var newRoute1 = new FlightRoute
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
            var newRoute2 = new FlightRoute
            {
                ArrivalLocation = "DEF",
                DepartureLocation = "GHI",
                NoOfStops = 0,
                PricePerPerson = 150.0f,
                SeatsAvailable = 50,
                DepartureDateTime = DateTime.Now.AddDays(1),
                ArrivalDateTime = DateTime.Now.AddDays(1).AddHours(2),
                FlightId = _flight.FlightId
            };
            await _routeRepository.Add(newRoute1);
            await _routeRepository.Add(newRoute2);

            // Act
            var result = await _routeRepository.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public void GetAll_Failure_NoRoutesPresent()
        {
            // Arrange

            // Act & Assert
            Assert.ThrowsAsync<RouteException>(() => _routeRepository.GetAll());
        }
    }
}
