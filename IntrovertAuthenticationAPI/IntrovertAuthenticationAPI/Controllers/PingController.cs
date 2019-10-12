/* 
 * Author: Smailovic Alen
 * Date:   01.09.2019
*/

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Reflection;
using System.Threading;

namespace IntrovertAuthenticationAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class PingController : ControllerBase
    {
        private const string BUILD_NUMBER_ENV_VARIABLE = "BUILD_NUMBER";
        private const string DATE_TIME_FORMAT = "yyyy-MM-dd HH:mm:ss.fff zzz";
        private const string TOTAL_COUNT_HEADER_KEY = "X-Total-Count";

        private readonly IHostingEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of PingController
        /// </summary>
        /// <param name="environment"></param>
        public PingController(IHostingEnvironment environment)
        {
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
        }

        /// <summary>
        /// Returns a ping message to see if authentication service is running.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AllowAnonymous]
        [ApiExplorerSettings(IgnoreApi = true)]
        public IActionResult Ping()
        {
            return Ok(new
            {
                Ping = true,
                LocalTime = DateTime.Now.ToString(DATE_TIME_FORMAT),
                Controller = GetType().Name,
                Environment = _environment.EnvironmentName,
                BuildNumber = Environment.GetEnvironmentVariable(BUILD_NUMBER_ENV_VARIABLE),
                BuildTime = GetLinkerTime(Assembly.GetExecutingAssembly()).ToString(DATE_TIME_FORMAT),
                CultureName = Thread.CurrentThread.CurrentCulture.Name
            });
        }

        /// <summary>
        /// Gets the linker date of the assembly.
        /// </summary>
        /// <param name="assembly"></param>
        /// <returns></returns>
        /// <remarks>https://blog.codinghorror.com/determining-build-date-the-hard-way/</remarks>
        private static DateTime GetLinkerTime(Assembly assembly)
        {
            try
            {
                string filePath = assembly.Location;
                const int peHeaderOffset = 60;
                const int linkerTimestampOffset = 8;

                byte[] buffer = new byte[2048];

                using (FileStream stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                {
                    stream.Read(buffer, 0, 2048);
                }

                int offset = BitConverter.ToInt32(buffer, peHeaderOffset);
                int secondsSince1970 = BitConverter.ToInt32(buffer, offset + linkerTimestampOffset);
                DateTime epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

                return epoch.AddSeconds(secondsSince1970);
            }
            catch
            {
                return DateTime.MinValue;
            }
        }
    }
}
