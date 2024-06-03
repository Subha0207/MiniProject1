using FlightManagementSystemAPI.Contexts;
using FlightManagementSystemAPI.Interfaces;
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

namespace FlightManagementUnitTests.ServiceTests
{
    public class RefundServiceTest
    {
        private FlightManagementContext _context;
        private FlightRepository _flightRepository;
        private RouteRepository _routeRepository;
        private BookingRepository _bookingRepository;
        private PaymentRepository _paymentRepository;
        private PaymentService _paymentService;
        private CancellationRepository _cancellationRepository;
        private RefundRepository _refundRepository;
        private RefundService _refundService;

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
            var bookingLoggerMock = new Mock<ILogger<BookingRepository>>();
            _bookingRepository = new BookingRepository(_context, bookingLoggerMock.Object);
            var paymentLoggerMock = new Mock<ILogger<PaymentRepository>>();
            _paymentRepository = new PaymentRepository(_context, paymentLoggerMock.Object);

            var cancellationLoggerMock = new Mock<ILogger<CancellationRepository>>();
            _cancellationRepository = new CancellationRepository(_context, cancellationLoggerMock.Object);

            var refundLoggerMock = new Mock<ILogger<RefundRepository>>();
            _refundRepository = new RefundRepository(_context, refundLoggerMock.Object);

            var refundServiceLoggerMock = new Mock<ILogger<RefundService>>();
            _refundService = new RefundService(_cancellationRepository,_refundRepository,refundServiceLoggerMock.Object);
        }


        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}
