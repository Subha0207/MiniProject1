using FlightManagementSystemAPI.Contexts;
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
using FlightManagementSystemAPI.Interfaces;

namespace FlightManagementUnitTests.ServiceTests
{
    public class SubRouteServiceTest
    {


        private FlightManagementContext _context;
        private FlightRepository _flightRepository;
        private FlightService _flightService;
        private RouteRepository _routeRepository;
        private SubRouteRepository _subrouteRepository;
        private SubRouteService _subrouteService;

        [SetUp]
        public async Task Setup()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder()
                                .UseInMemoryDatabase("dummyDB");
            _context = new FlightManagementContext(optionsBuilder.Options);

            _context.Database.EnsureDeletedAsync().Wait();
            _context.Database.EnsureCreatedAsync().Wait();


            // Initial Setup


            var flightLoggerMock = new Mock<ILogger<FlightRepository>>();
            _flightRepository = new FlightRepository(_context, flightLoggerMock.Object);

            var routeLoggerMock = new Mock<ILogger<RouteRepository>>();
            _routeRepository = new RouteRepository(_context, routeLoggerMock.Object);

            var subrouteLoggerMock = new Mock<ILogger<SubRouteRepository>>();
            _subrouteRepository = new SubRouteRepository(_context, subrouteLoggerMock.Object);
            var subrouteServiceLoggerMock = new Mock<ILogger<SubRouteService>>();
            _subrouteService = new SubRouteService(
                subrouteServiceLoggerMock.Object,
                _subrouteRepository,
                _flightRepository,
                _routeRepository
            );
        }
        [Test]
        public async Task Add_SubRoutes_Success()
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
            var _addedflight = await _flightRepository.Add(flight);
            var routeDTO = new RouteDTO
            {
                FlightId = _addedflight.FlightId,
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
                FlightId = _addedflight.FlightId,
                ArrivalDateTime = routeDTO.ArrivalDateTime,
                ArrivalLocation = routeDTO.ArrivalLocation,
                DepartureLocation = routeDTO.DepartureLocation,
                NoOfStops = routeDTO.NoOfStops,
                PricePerPerson = routeDTO.PricePerPerson,
                DepartureDateTime = routeDTO.DepartureDateTime,
                SeatsAvailable = routeDTO.SeatsAvailable
            };
            var _route = await _routeRepository.Add(addedRoute);
            var subRouteDTOs = new SubRouteDTO[]
            {
        new SubRouteDTO
        {
            FlightId = _addedflight.FlightId,
            RouteId = _route.RouteId,
            Stops = new StopDTO[]
            {
                new StopDTO
                {
                    SubFlightId = 1,
                    ArrivalLocation = "",
                    ArrivalDateTime = DateTime.Now.AddHours(2),
                    DepartureLocation = "abc",
                    DepartureDateTime = DateTime.Now
                },
                new StopDTO
                {
                    SubFlightId = 2,
                    ArrivalLocation = "xyz",
                    ArrivalDateTime = DateTime.Now.AddHours(4),
                    DepartureLocation = "def",
                    DepartureDateTime = DateTime.Now.AddHours(3)
                }
            }
        }
            };

            // Act
            var result = await _subrouteService.AddSubRoutes(subRouteDTOs);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(subRouteDTOs.Length, result.Length);
            for (int i = 0; i < subRouteDTOs.Length; i++)
            {
                Assert.AreEqual(subRouteDTOs[i].FlightId, result[i].FlightId);
                Assert.AreEqual(subRouteDTOs[i].RouteId, result[i].RouteId);
                Assert.AreEqual(subRouteDTOs[i].Stops[0].ArrivalLocation, result[i].ArrivalLocation);
                Assert.AreEqual(subRouteDTOs[i].Stops[0].DepartureLocation, result[i].DepartureLocation);
                Assert.AreEqual(subRouteDTOs[i].Stops[0].ArrivalDateTime, result[i].ArrivalDateTime);
                Assert.AreEqual(subRouteDTOs[i].Stops[0].DepartureDateTime, result[i].DepartureDateTime);
            }
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
