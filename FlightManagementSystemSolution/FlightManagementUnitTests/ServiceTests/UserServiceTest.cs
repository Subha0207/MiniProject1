using FlightManagementSystemAPI.Contexts;
using FlightManagementSystemAPI.Exceptions.UserExceptions;
using FlightManagementSystemAPI.Interfaces;
using FlightManagementSystemAPI.Model;
using FlightManagementSystemAPI.Model.DTOs;
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
using System.Numerics;
using System.Security.Cryptography;
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
        private UserService _userService;

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

            var userLoggerMock1 = new Mock<ILogger<UserService>>();
            _userService = new UserService(_userRepository, _userInfoRepository, _tokenService, userLoggerMock1.Object);



        }

        [Test]
        public async Task Register_ValidData_SuccessfullyRegistersUser()
        {
            // Arrange
            var registerDTO = new RegisterDTO
            {
                Name = "John Doe",
                Email = "john@example.com",
                Password = "password",
                ConfirmPassword = "password",
                Role = "User"
            };

            // Act
            var result = await _userService.Register(registerDTO);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(registerDTO.Name, result.Name);
            Assert.AreEqual(registerDTO.Email, result.Email);
            Assert.AreEqual(registerDTO.Role, result.Role);
        }

        [Test]
        public async Task Register_InvalidData_ThrowsUserRepositoryException()
        {
            // Arrange
            var userRegisterDTO = new RegisterDTO
            {
                // Missing required fields
                Email = "john@example.com",
                Password = "password"
            };


            // Act & Assert
            Assert.ThrowsAsync<UserException>(async () => await _userService.Register(userRegisterDTO));

        }

        [Test]
        public async Task Register_ValidData_DuplicateEmail()
        {
            // Arrange
            var userRegisterDTO = new RegisterDTO
            {
                Name = "John Doe",
                Email = "john@example.com",
                Password = "password",
                ConfirmPassword="password",
                Role="user"
            };
            var userRegisterDTO1 = new RegisterDTO
            {
                Name = "John Doe",
                Email = "john@example.com",
                Password = "password",
                ConfirmPassword = "password",
                Role = "user"
            };

            // Act
            await _userService.Register(userRegisterDTO1);


            // Assert
            Assert.ThrowsAsync<UserException>(async () => await _userService.Register(userRegisterDTO));
        }

        [Test]
        public async Task Register_Exception()
        {
            // Arrange
            var userRegisterDTO = new RegisterDTO
            {
                Name = "John Doe",
                Email = "john@example.com",
                Password = "password",
                ConfirmPassword = "password",
                Role = "user"
            };


            // Act
            var repositoryMock = new Mock<IRepository<int, User>>();
            repositoryMock.Setup(repo => repo.Update(It.IsAny<User>())).ThrowsAsync(new Exception("Test exception"));
            var userMock = new UserService(repositoryMock.Object, _userInfoRepository, _tokenService, Mock.Of<ILogger<UserService>>());


            // Assert
            Assert.ThrowsAsync<UnableToRegisterException>(async () => await userMock.Register(userRegisterDTO));
        }

        [Test]
        public async Task Login_Success()
        {
            // Arrange
            var userRegisterDTO = new RegisterDTO
            {
                Name = "John Doe",
                Email = "john@example.com",
                Password = "password",
                ConfirmPassword = "password",
                Role = "user"
            };
            var result = await _userService.Register(userRegisterDTO);

            var loginDTO = new LoginDTO
            {
                UserId = result.UserId,
                Password = "password"
            };
            // Act
            var res = await _userService.Login(loginDTO);
            Assert.IsNotNull(res);
            Assert.IsNotEmpty(res.Token);
            Assert.AreEqual(result.UserId, res.UserId);
        }

        [Test]
        public async Task Login_WrongPassword()
        {
            // Arrange
            var userRegisterDTO = new RegisterDTO
            {
                Name = "John Doe",
                Email = "john@example.com",
                Password = "password",
                ConfirmPassword = "password",
                Role = "user"
            };
            var result = await _userService.Register(userRegisterDTO);

            var loginDTO = new LoginDTO
            {
                UserId = result.UserId,
                Password = "sdf"
            };
            // Act and assert
            Assert.ThrowsAsync<UnAuthorizedUserException>(async () => await _userService.Login(loginDTO));

        }
        [Test]
        public async Task Login_NoUser()
        {
            // Arrange
            var userRegisterDTO = new RegisterDTO
            {
                Name = "John Doe",
                Email = "john@example.com",
                Password = "password",
                
            };
            var result = await _userService.Register(userRegisterDTO);

            var loginDTO = new LoginDTO
            {
                UserId = 2,
                Password = "sdf"
            };
            // Act and assert
            Assert.ThrowsAsync<UserException>(async () => await _userService.Login(loginDTO));

        }
        [Test]

        public async Task ActivateUser()
        {
            User user = new User()
            {
               Name="Subha",
               Email="subha@gmail.com",
               ConfirmPassword="abc",
               Password="abc",
               Role="user"
            };
            await _userRepo.Add(user);
            UserInfo userInfo = new UserInfo()
            {
                UserId = 1,
                Password = Encoding.UTF8.GetBytes("yourPassword"),
                PasswordHashKey = SHA256.HashData(Encoding.UTF8.GetBytes("yourPassword")),
                Status = "Disable"
            };
            await _userInfoRepository.Add(userInfo);


            var result = await _userService.UserActivation(1);
            var UserInfo = await _userInfoRepository.Get(result);
            Assert.That(UserInfo.Status, Is.EqualTo("Active"));
        }




        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

    }

}
