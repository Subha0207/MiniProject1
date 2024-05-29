
using FlightManagementSystemAPI.Contexts;
using FlightManagementSystemAPI.Exceptions.UserExceptions;
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
    public class UserDetailRepositoryTests
    {
        private FlightManagementContext _context;
        private UserInfoRepository _userInfoRepository;
        private UserRepository _userRepository;
        private User _user;

        [SetUp]
        public async Task Setup()
        {
            var options = new DbContextOptionsBuilder<FlightManagementContext>()
                .UseInMemoryDatabase(databaseName: "dummyDB")
                .Options;
            _context = new FlightManagementContext(options);

            var userDetailLoggerMock = new Mock<ILogger<UserInfoRepository>>();
            var userLoggerMock = new Mock<ILogger<UserRepository>>();
            _userInfoRepository = new UserInfoRepository(_context, userDetailLoggerMock.Object);
            _userRepository = new UserRepository(_context, userLoggerMock.Object);


            // Initialize User
            _user = await _userRepository.Add(new User
            {
                Name = "John",
                Email = "john@example.com",
               ConfirmPassword="123",
                Password = "123",
                Role = "User"
            });
        }

        [Test]
        public async Task Add_Success()
        {
            // Arrange
            var newUserDetail = new UserInfo
            {
                UserId = _user.UserId,
                Email = "john@example.com",
                Password = new byte[] { 1, 2, 3 },
                PasswordHashKey = new byte[] { 4, 5, 6 },
                Status="active"
            };

            // Act
            var result = await _userInfoRepository.Add(newUserDetail);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual("john@example.com", result.Email);
        }

        [Test]
        public void Add_Failure_DuplicateUserException()
        {
            // Arrange
            var userDetail = new UserInfo
            {
                UserId = _user.UserId,
                Email = "john@example.com",
                Password = new byte[] { 1, 2, 3 },
                PasswordHashKey = new byte[] { 4, 5, 6 },
                Status = "active"
            };
            _userInfoRepository.Add(userDetail).Wait();

            var duplicateUserDetail = new UserInfo
            {
                UserId = _user.UserId + 1,
                Email = "john@example.com",
                Password = new byte[] { 1, 2, 3 },
                PasswordHashKey = new byte[] { 4, 5, 6 },
                Status = "active"
            };

            // Act & Assert
            Assert.ThrowsAsync<UserInfoException>(() => _userInfoRepository.Add(duplicateUserDetail));
        }

        [Test]
        public void Add_Failure_Exception()
        {
            // Arrange
            var invalidUserDetail = new UserInfo();

            // Act & Assert
            Assert.ThrowsAsync<UserInfoException>(() => _userInfoRepository.Add(invalidUserDetail));
        }

        [Test]
        public async Task GetByKey_Success()
        {
            // Arrange
            var newUserDetail = new UserInfo
            {
                UserId = _user.UserId,
                Email = "john@example.com",
                Password = new byte[] { 1, 2, 3 },
                PasswordHashKey = new byte[] { 4, 5, 6 },
                Status = "active"
            };
            await _userInfoRepository.Add(newUserDetail);

            // Act
            var result = await _userInfoRepository.Get(_user.UserId);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(_user.UserId, result.UserId);
        }

        [Test]
        public void GetByKey_Failure_NotFoundException()
        {
            // Arrange
            var nonExistentUserId = 999; // Assuming invalid user ID

            // Act & Assert
            Assert.ThrowsAsync<UserInfoException>(() => _userInfoRepository.Get(nonExistentUserId));
        }

        [Test]
        public async Task Update_Success()
        {
            // Arrange
            var newUserDetail = new UserInfo
            {
                UserId = _user.UserId,
                Email = "john@example.com",
                Password = new byte[] { 1, 2, 3 },
                PasswordHashKey = new byte[] { 4, 5, 6 },
                Status = "active"
            };
            await _userInfoRepository.Add(newUserDetail);

            // Modify some data in the user detail
            newUserDetail.Email = "updated@example.com";

            // Act
            var result = await _userInfoRepository.Update(newUserDetail);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual("updated@example.com", result.Email);
        }

        [Test]
        public async Task Update_Failure_UserDetailNotFound()
        {
            // Arrange
            var nonExistentUserDetail = new UserInfo
            {
                UserId = 999, // Assuming invalid user ID
                Email = "nonexistent@example.com",
                Password = new byte[] { 1, 2, 3 },
                PasswordHashKey = new byte[] { 4, 5, 6 },
                Status = "active"
            };

            // Act & Assert
            Assert.ThrowsAsync<UserInfoException>(async () => await _userInfoRepository.Update(nonExistentUserDetail));
        }

        [Test]
        public async Task DeleteByKey_Success()
        {
            // Arrange
            var newUserDetail = new UserInfo
            {
                UserId = _user.UserId,
                Email = "john@example.com",
                Password = new byte[] { 1, 2, 3 },
                PasswordHashKey = new byte[] { 4, 5, 6 },
                Status = "active"
            };
            await _userInfoRepository.Add(newUserDetail);

            // Act
            var result = await _userInfoRepository.Delete(_user.UserId);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(_user.UserId, result.UserId);
        }

        [Test]
        public void DeleteByKey_Failure_UserDetailNotFound()
        {
            // Arrange
            var nonExistentUserId = 999;

            // Act & Assert
            Assert.ThrowsAsync<UserInfoException>(async () => await _userInfoRepository.Delete(nonExistentUserId));
        }

        [Test]
        public async Task GetAll_Success()
        {
            // Arrange
            var newUserDetail1 = new UserInfo
            {
                UserId = _user.UserId,
                Email = "john@example.com",
                Password = new byte[] { 1, 2, 3 },
                PasswordHashKey = new byte[] { 4, 5, 6 },
                Status = "active"
            };
            var newUserDetail2 = new UserInfo
            {
                UserId = _user.UserId + 1,
                Email = "jane@example.com",
                Password = new byte[] { 7, 8, 9 },
                PasswordHashKey = new byte[] { 10, 11, 12 },
                Status = "active"
            };
            await _userInfoRepository.Add(newUserDetail1);
            await _userInfoRepository.Add(newUserDetail2);

            // Act
            var result = await _userInfoRepository.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public void GetAll_Failure_NoUserDetailsPresent()
        {
            // Arrange 

            // Act & Assert
            Assert.ThrowsAsync<UserInfoException>(async () => await _userInfoRepository.GetAll());
        }


        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}