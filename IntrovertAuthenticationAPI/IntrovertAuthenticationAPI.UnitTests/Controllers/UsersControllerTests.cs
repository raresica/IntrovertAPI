/* 
 * Author: Smailovic Alen
 * Date:   01.09.2019
*/

using AutoMapper;
using IntrovertAuthenticationAPI.Controllers;
using IntrovertAuthenticationAPI.DTOs;
using IntrovertAuthenticationAPI.Entities;
using IntrovertAuthenticationAPI.Processors;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;

namespace IntrovertAuthenticationAPI.UnitTests.Controllers
{
    [TestFixture]
    class UsersControllerTests
    {
        private Mock<IMapper> _mockMapper;
        private Mock<IUserProcessor> _mockUserProcessor;

        private UsersController _usersController;

        [SetUp]
        public void Setup()
        {
            _mockMapper = new Mock<IMapper>();
            _mockUserProcessor = new Mock<IUserProcessor>();

            _usersController = new UsersController(_mockUserProcessor.Object, _mockMapper.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _usersController = null;

            _mockMapper.Reset();
            _mockUserProcessor.Reset();
        }

        [Test]
        public void RegisterNullEntityTest()
        {
            // Arrange
            UserRxDto user = new UserRxDto
            {
                Username = "test",
                Phone = "0000",
                Email = "test@test.com",
                Password = "test"
            };

            _mockMapper.Setup(m => m.Map<UserEntity>(It.IsAny<UserRxDto>())).Returns((UserEntity)null).Verifiable();
            _mockUserProcessor.Setup(m => m.Create(It.IsAny<UserEntity>(), It.IsAny<string>())).ReturnsAsync(It.IsAny<UserEntity>()).Verifiable();


            // Act
            IActionResult result = _usersController.Register(user).Result;

            // Assert
            Assert.IsNotNull(result);
            _mockMapper.Verify(m => m.Map<UserEntity>(user), Times.Once);
            _mockUserProcessor.Verify(m => m.Create(It.IsAny<UserEntity>(), user.Password), Times.Once);
        }
    }
}
