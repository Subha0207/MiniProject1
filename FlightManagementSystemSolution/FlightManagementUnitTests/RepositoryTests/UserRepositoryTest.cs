
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
    public class UserRepositoryTests
    {
        private FlightManagementContext _context;
        private UserRepository _userRepository;

        [SetUp]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<FlightManagementContext>()
                .UseInMemoryDatabase(databaseName: "dummyDB")
                .Options;
            _context = new FlightManagementContext(options);

            var userLoggerMock = new Mock<ILogger<UserRepository>>();
            _userRepository = new UserRepository(_context, userLoggerMock.Object);

        }

        [Test]
        public async Task Add_Success()
        {
            // Arrange
            var newUser = new User
            {
                Name = "Subhashini",
                Email = "subha@example.com",
                Password = "123",
                ConfirmPassword = "123",
                Role = "User"
            };


            // Act
            var result = await _userRepository.Add(newUser);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual("Subhashini", result.Name);
        }

        [Test]
        public void Add_Failure_DuplicateUserException_Email()
        {
            // Arrange
            var user = new User
            {
                Name = "John Doe",
                Email = "john@example.com",
                Password = "123",
                ConfirmPassword = "123",
                Role = "User"
            };
            _userRepository.Add(user).Wait();
            var duplicateUser = new User
            {
                Name = "Jane Doe",
                Email = "john@example.com", // Same email
                Password = "123",
                ConfirmPassword = "123",
                Role = "User"
            };

            // Act & Assert
           var ex= Assert.ThrowsAsync<UserException>(() => _userRepository.Add(duplicateUser));
            Assert.That(ex.InnerException, Is.TypeOf<UserAlreadyExistsException>());
            Assert.That(ex.InnerException.Message, Is.EqualTo("A user with the same email already exists."));


        }

        [Test]
        public void Add_Failure_PasswordMismatchException()
        {
            // Arrange
            var user = new User
            {
                Name = "Mridu",
                Email = "Mridu@example.com",
                Password = "123",
                ConfirmPassword = "456", // Password and ConfirmPassword do not match
                Role = "Customer"
            };

            var ex = Assert.ThrowsAsync<UserException>(() => _userRepository.Add(user));
            Assert.That(ex.InnerException, Is.TypeOf<PasswordException>());
            Assert.That(ex.InnerException.Message, Is.EqualTo("Password and confirm password must be the same."));
        }

        [Test]
        public void Add_Failure_InvalidEmailFormatException()
        {
            var user = new User
            {
                Name = "John Doe",
                Email = "john.com",// Invalid Mail format
                Password = "123",
                ConfirmPassword = "123", 
                Role = "Customer"
            };

            var ex = Assert.ThrowsAsync<UserException>(() => _userRepository.Add(user));
            Assert.That(ex.InnerException, Is.TypeOf<EmailFormatException>());
            Assert.That(ex.InnerException.Message, Is.EqualTo("Invalid email format."));
        }
        [Test]
        public void Add_Failure_Exception()
        {
            // Arrange
            var invalidUser = new User();

            // Act & Assert
            Assert.ThrowsAsync<UserException>(() => _userRepository.Add(invalidUser));
        }

        [Test]
        public async Task GetByKey_Success()
        {
            // Arrange
            var uniqueEmail = $"john{Guid.NewGuid()}@example.com";
            var newUser = new User
            {
                Name = "John Doe",
                Email = uniqueEmail,
                Password = "123",
                ConfirmPassword = "123",
                Role = "User"
            };
            var addedUser = await _userRepository.Add(newUser);

            // Act
            var result = await _userRepository.Get(addedUser.UserId);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(addedUser.UserId, result.UserId);
        }

        [Test]
        public void GetByKey_Failure_NotFoundException()
        {
            // Arrange
            var nonExistentUserId = 999;

            // Act & Assert
            Assert.ThrowsAsync<UserException>(() => _userRepository.Get(nonExistentUserId));
        }

        [Test]
        public async Task Update_Success()
        {
            // Arrange
            var newUser = new User
            {
                Name = "John Doe",
                Email = "john@example.com",
                Password = "123",
                ConfirmPassword = "123",
                Role = "User"
            };

            var addedUser = await _userRepository.Add(newUser);

            // Modify some data in the user
            addedUser.Name = "Updated John Doe";

            // Act
            var result = await _userRepository.Update(addedUser);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual("Updated John Doe", result.Name);
        }

        [Test]
        public async Task Update_Failure_UserNotFound()
        {
            // Arrange
            var nonExistentUser = new User
            {
                UserId = 999,
                Name = "Non-existent User",
                Email = "nonexistent@example.com",
                Password = "123",
                ConfirmPassword = "123",
                Role = "User"
            };

            // Act & Assert
            Assert.ThrowsAsync<UserException>(async () => await _userRepository.Update(nonExistentUser));
        }

        [Test]
        public async Task DeleteByKey_Success()
        {
            // Arrange
         
            var newUser = new User
            {
                Name = "John Doe",
                Email = $"john{Guid.NewGuid()}@example.com",
                Password = "123",
                ConfirmPassword = "123",
                Role = "User"
            };

            var addedUser = await _userRepository.Add(newUser);

            // Act
            var result = await _userRepository.Delete(addedUser.UserId);

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(addedUser.UserId, result.UserId);
        }

        [Test]
        public void DeleteByKey_Failure_UserNotFound()
        {
            // Arrange
            var nonExistentUserId = 999;

            // Act & Assert
            Assert.ThrowsAsync<UserException>(async () => await _userRepository.Delete(nonExistentUserId));
        }

        [Test]
        public async Task GetAll_Success()
        {
            // Arrange
            var newUser1 = new User
            {
                Name = "John Doe",
                Email = "john@example.com",
                Password = "123",
                ConfirmPassword = "123",
                Role = "User"
            };

            var newUser2 = new User
            {
                Name = "John Doe",
                Email = "john1@example.com",
                Password = "123",
                ConfirmPassword = "123",
                Role = "User"
            };

            await _userRepository.Add(newUser1);
            await _userRepository.Add(newUser2);

            // Act
            var result = await _userRepository.GetAll();

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual(2, result.Count());
        }

        [Test]
        public void GetAll_Failure_NoUsersPresent()
        {
            // Arrange

            // Act & Assert
            Assert.ThrowsAsync<UserException>(async () => await _userRepository.GetAll());
        }


        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }
    }
}