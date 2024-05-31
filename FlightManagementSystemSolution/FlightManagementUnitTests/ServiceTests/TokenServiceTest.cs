using Castle.Core.Configuration;
using FlightManagementSystemAPI.Model;
using FlightManagementSystemAPI.Services;
using Microsoft.Extensions.Configuration;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IConfiguration = Microsoft.Extensions.Configuration.IConfiguration;

namespace FlightManagementUnitTests.ServiceTests
{
    public class TokenServiceTest
    {
        private Mock<IConfiguration> _mockConfig;
        private TokenService _tokenService;


        [SetUp]
        public void Setup()
        {
            _mockConfig = new Mock<IConfiguration>();
            _mockConfig.Setup(x => x.GetSection("TokenKey").GetSection("JWT").Value).Returns("This is the dummy key which has to be a bit long for the 512. which should be even more longer for the passing");
            _tokenService = new TokenService(_mockConfig.Object);
        }

        [Test]
        public void GenerateTokenTest()
        {

            //Arrange
            var user = new User { UserId = 103, Role = "User", Email = "test@example.com" };

            //Action
            var token = _tokenService.GenerateToken(user);

            //Assert
            Assert.IsNotNull(token);
            Assert.IsNotEmpty(token);
        }

    }
}
