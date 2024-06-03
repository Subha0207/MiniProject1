using FlightManagementSystemAPI.Contexts;
using FlightManagementSystemAPI.Exceptions.RouteExceptions;
using FlightManagementSystemAPI.Interfaces;
using FlightManagementSystemAPI.Model;
using FlightManagementSystemAPI.Model.DTOs;
using FlightManagementSystemAPI.Repositories;
using FlightManagementSystemAPI.Services;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FlightManagementUnitTests.ServiceTests
{
    public class RouteServiceTest
    {
        private FlightManagementContext _context;
        private FlightRepository _flightRepository;
        private RouteRepository _routeRepository;
        private RouteService _routeService;

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

            var routeLoggerMockService = new Mock<ILogger<RouteService>>();
            _routeService = new RouteService(_routeRepository, _flightRepository, routeLoggerMockService.Object);
        }

        [Test]
        public async Task Add_Route_Success()
        {
            // Arrange
            var flightDTO = new FlightDTO
            {
                FlightName = "abc",
                SeatCapacity = 20
            };

            // Convert DTO to Model
            var flight = new Flight
            {
                FlightName = flightDTO.FlightName,
                SeatCapacity = flightDTO.SeatCapacity
            };

            // Act
            var _flight = await _flightRepository.Add(flight);
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

            // Act
            var result = await _routeService.AddRoute(routeDTO);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(routeDTO.SeatsAvailable, result.SeatsAvailable);
            Assert.AreEqual(routeDTO.DepartureLocation, result.DepartureLocation);
            Assert.AreEqual(routeDTO.ArrivalLocation, result.ArrivalLocation);
            Assert.AreEqual(routeDTO.DepartureDateTime, result.DepartureDateTime);
            Assert.AreEqual(routeDTO.ArrivalDateTime, result.ArrivalDateTime);
            Assert.AreEqual(routeDTO.NoOfStops, result.NoOfStops);
            Assert.AreEqual(routeDTO.PricePerPerson, result.PricePerPerson);
        }

        [Test]
        public async Task Add_Route_Failure()
        {
            // Arrange
            var routeDTO = new RouteDTO
            {
                FlightId = -1, // Invalid FlightId
                ArrivalDateTime = DateTime.Now.AddHours(2),
                ArrivalLocation = "xyz",
                DepartureLocation = "abc",
                NoOfStops = 1,
                PricePerPerson = 5000,
                DepartureDateTime = DateTime.Now,
                SeatsAvailable = 20
            };

            // Act & Assert
            Assert.ThrowsAsync<RouteServiceException>(async () => await _routeService.AddRoute(routeDTO));
        }

        [Test]
        public async Task Get_Route_Success()
        {
            // Arrange
            var flight = new Flight
            {
                FlightName = "abc",
                SeatCapacity = 20
            };
            var _flight = await _flightRepository.Add(flight);

            var route = new FlightRoute
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
            var _route = await _routeRepository.Add(route);

            // Act
            var result = await _routeService.GetRoute(_route.RouteId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(_route.RouteId, result.RouteId);
        }

        [Test]
        public async Task Get_Route_Failure()
        {
            // Act & Assert
            Assert.ThrowsAsync<RouteException>(async () => await _routeService.GetRoute(-1));
        }

        [Test]
        public async Task Get_All_Routes_Success()
        {
            // Arrange
            var flight = new Flight
            {
                FlightName = "abc",
                SeatCapacity = 20
            };
            var _flight = await _flightRepository.Add(flight);

            var route1 = new FlightRoute
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
            await _routeRepository.Add(route1);

            var route2 = new FlightRoute
            {
                FlightId = _flight.FlightId,
                ArrivalDateTime = DateTime.Now.AddHours(3),
                ArrivalLocation = "def",
                DepartureLocation = "uvw",
                NoOfStops = 2,
                PricePerPerson = 6000,
                DepartureDateTime = DateTime.Now.AddHours(1),
                SeatsAvailable = 30
            };
            await _routeRepository.Add(route2);

            // Act
            var result = await _routeService.GetAllRoutes();

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public async Task Get_All_Routes_Failure()
        {
          
            
            // Act & Assert
            var ex = Assert.ThrowsAsync<RouteException>(async () => await _routeService.GetAllRoutes());
            Assert.AreEqual("Error while getting routes: No route is present.", ex.Message);
        }
        [Test]
        public async Task Update_Route_Success()
        {
            // Arrange
            var routeDTO = new RouteReturnDTO
            {
                RouteId = 1,
                FlightId = 101,
                ArrivalDateTime = DateTime.Now.AddHours(3),
                ArrivalLocation = "LocationB",
                DepartureDateTime = DateTime.Now,
                DepartureLocation = "LocationA",
                NoOfStops = 2,
                SeatsAvailable = 30,
                PricePerPerson = 1500
            };

            var existingRoute = new FlightRoute
            {
                RouteId = 1,
                FlightId = 100,
                ArrivalDateTime = DateTime.Now.AddHours(1),
                ArrivalLocation = "OldLocationB",
                DepartureDateTime = DateTime.Now.AddHours(-1),
                DepartureLocation = "OldLocationA",
                NoOfStops = 1,
                SeatsAvailable = 20,
                PricePerPerson = 1000
            };

            // Simulate adding existing route to the repository
            await _routeRepository.Add(existingRoute);

            // Act
            var result = await _routeService.UpdateRoute(routeDTO);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(routeDTO.RouteId, result.RouteId);
            Assert.AreEqual(routeDTO.FlightId, result.FlightId);
            Assert.AreEqual(routeDTO.ArrivalDateTime, result.ArrivalDateTime);
            Assert.AreEqual(routeDTO.ArrivalLocation, result.ArrivalLocation);
            Assert.AreEqual(routeDTO.DepartureDateTime, result.DepartureDateTime);
            Assert.AreEqual(routeDTO.DepartureLocation, result.DepartureLocation);
            Assert.AreEqual(routeDTO.NoOfStops, result.NoOfStops);
            Assert.AreEqual(routeDTO.SeatsAvailable, result.SeatsAvailable);
            Assert.AreEqual(routeDTO.PricePerPerson, result.PricePerPerson);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
