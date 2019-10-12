/* 
 * Author: Smailovic Alen
 * Date:   01.09.2019
*/

using IntrovertAuthenticationAPI.Entities;
using IntrovertAuthenticationAPI.Processors;
using IntrovertAuthenticationAPI.Services;
using Moq;
using NUnit.Framework;

namespace IntrovertAuthenticationAPI.UnitTests.Processors
{
    [TestFixture]
    class UserProcessorTests
    {
        private Mock<IUserService> _mockUserService;

        private UserProcessor _usersProcessor;

        [SetUp]
        public void Setup()
        {
            _mockUserService = new Mock<IUserService>();

            _usersProcessor = new UserProcessor(_mockUserService.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _usersProcessor = null;

            _mockUserService.Reset();
        }

        [Test]
        public void CreateUserTest()
        {
            UserEntity user = new UserEntity();

            _mockUserService.Setup(m => m.CreateUser(It.IsAny<UserEntity>())).Verifiable();

            UserEntity result = _usersProcessor.Create(user, "password").Result;

            Assert.AreNotEqual(user, result);
            _mockUserService.Verify(m => m.CreateUser(user), Times.Once);
        }
    }
}
