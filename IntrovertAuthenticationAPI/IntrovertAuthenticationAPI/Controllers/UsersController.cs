/* 
 * Author: Smailovic Alen
 * Date:   01.09.2019
*/

using AutoMapper;
using IntrovertAuthenticationAPI.DTOs;
using IntrovertAuthenticationAPI.Entities;
using IntrovertAuthenticationAPI.Processors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace IntrovertAuthenticationAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/v1/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserProcessor _userProcessor;
        private readonly IMapper _mapper;

        public UsersController(IUserProcessor userProcessor, IMapper mapper)
        {
            _userProcessor = userProcessor ?? throw new ArgumentNullException();
            _mapper = mapper ?? throw new ArgumentNullException();
        }

        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] UserRxDto userReceivedDto)
        {
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRxDto userReceivedDto)
        {
            // map dto to entity
            UserEntity user = _mapper.Map<UserEntity>(userReceivedDto);

            try
            {
                // save 
                await _userProcessor.Create(user, userReceivedDto.Password);
                return Ok();
            }
            catch (Exception ex)
            {
                // return error message if there was an exception
                return BadRequest(new { message = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([Required] string id)
        {
            return Ok(new UserTxDto());
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update([Required] string id, [FromBody]UserRxDto userReceivedDto)
        {
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([Required] string id)
        {
            return Ok();
        }
    }
}
