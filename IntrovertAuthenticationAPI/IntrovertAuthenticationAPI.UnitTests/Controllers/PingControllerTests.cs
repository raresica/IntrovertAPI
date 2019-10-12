/* 
 * Author: Smailovic Alen
 * Date:   01.09.2019
*/

using IntrovertAuthenticationAPI.Controllers;
using Microsoft.AspNetCore.Hosting;
using Moq;
using NUnit.Framework;
using System;

namespace IntrovertAuthenticationAPI.UnitTests.Controllers
{
    class PingControllerTests
    {
        private PingController _pingController;
        private Mock<IHostingEnvironment> _hostingEnvironment;

        [SetUp]
        public void Setup()
        {
            _hostingEnvironment = new Mock<IHostingEnvironment>();
            _pingController = new PingController(_hostingEnvironment.Object);
        }

        [TearDown]
        public void TearDown()
        {
            _pingController = null;
            _hostingEnvironment.Reset();
        }

        [Test]
        public void PingControllerTest()
        {
            // Act
            var ex = Assert.Throws<ArgumentNullException>(() =>
            {
                PingController dummy = new PingController(null);
            });
            Assert.AreEqual("environment", ex.ParamName);
        }
    }
}
