using FlightManagementSystemAPI.Contexts;
using FlightManagementSystemAPI.Repositories;
using FlightManagementSystemAPI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace FlightManagementUnitTests.ServiceTests
{
    public class UserServiceTest
    {
        private Mock<IConfiguration> _mockConfig;
        private TokenService _tokenService;
        private UserRepository _userRepo;
        private UserRepository _userRepository;
        private FlightManagementContext _context;
        private UserInfoRepository _userInfoRepository;

        [SetUp]
        public async Task Setup()
        {
            DbContextOptionsBuilder optionsBuilder = new DbContextOptionsBuilder()
                                .UseInMemoryDatabase("dummyDB");
            _context = new FlightManagementContext(optionsBuilder.Options);

            _context.Database.EnsureDeletedAsync().Wait();
            _context.Database.EnsureCreatedAsync().Wait();

            //Token Services


            _mockConfig = new Mock<IConfiguration>();
            _mockConfig.Setup(x => x.GetSection("TokenKey").GetSection("JWT").Value).Returns("This is the dummy key which has to be a bit long for the 512. which should be even more longer for the passing");
            _tokenService = new TokenService(_mockConfig.Object);

            // Initial Setup

         
            var userLoggerMock = new Mock<ILogger<UserRepository>>();
            _userRepository = new UserRepository(_context, userLoggerMock.Object);
            var userInfoLoggerMock = new Mock<ILogger<UserInfoRepository>>();
            _userInfoRepository = new UserInfoRepository(_context, userInfoLoggerMock.Object);
          



        }
        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

    }

}
