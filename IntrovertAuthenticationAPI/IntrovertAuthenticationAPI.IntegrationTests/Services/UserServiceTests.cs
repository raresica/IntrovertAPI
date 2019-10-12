/* 
 * Author: Smailovic Alen
 * Date:   01.09.2019
*/

using IntrovertAuthenticationAPI.Entities;
using IntrovertAuthenticationAPI.IntegrationTests.Helper;
using IntrovertAuthenticationAPI.Services;
using IntrovertAuthenticationAPI.Settings;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IntrovertAuthenticationAPI.IntegrationTests.Services
{
    [TestFixture]
    class UserServiceTests
    {
        private UserService _userService;

        [SetUp]
        public void Setup()
        {
            IdentityDatabase identityDatabase = ConnectionHelper.Create();

            _userService = new UserService(identityDatabase);
        }

        [TearDown]
        public void TearDown()
        {
            _userService = null;
        }

        [Test]
        public void CreateUserAsyncTest()
        {
            UserEntity userEntity = new UserEntity
            {
                Username = "test",
                Email = "test@test.com",
                PasswordHash = Encoding.ASCII.GetBytes(""),
                PasswordSalt = Encoding.ASCII.GetBytes(""),
                Phone = "0000",
                Status = true,
            };

            _userService.CreateUser(userEntity).Wait();

            List<UserEntity> user = _userService.GetAllUsers().Result;
            _userService.RemoveUser(user.First().Id).Wait();

            List<UserEntity> result = _userService.GetAllUsers().Result;
            Assert.IsNotNull(result);
            Assert.Zero(result.Count);
        }

        [Test]
        public void RemoveUserAsyncTest()
        {
            UserEntity userEntity = new UserEntity
            {
                Username = "test",
                Email = "test@test.com",
                PasswordHash = Encoding.ASCII.GetBytes(""),
                PasswordSalt = Encoding.ASCII.GetBytes(""),
                Phone = "0000",
                Status = true,
            };

            _userService.CreateUser(userEntity).Wait();

            List<UserEntity> user = _userService.GetAllUsers().Result;
            _userService.RemoveUser(user.First().Id).Wait();

            List<UserEntity> result = _userService.GetAllUsers().Result;
            Assert.IsNotNull(result);
            Assert.Zero(result.Count);
        }
    }
}
