/* 
 * Author: Smailovic Alen
 * Date:   01.09.2019
*/

using System;
using System.Threading.Tasks;
using IntrovertAuthenticationAPI.Entities;
using IntrovertAuthenticationAPI.Services;

namespace IntrovertAuthenticationAPI.Processors
{
    public class UserProcessor : IUserProcessor
    {
        private readonly IUserService _userService;

        public UserProcessor (IUserService userService)
        {
            _userService = userService ?? throw new ArgumentNullException();
        }

        public UserEntity Authenticate(string username, string password)
        {
            throw new NotImplementedException();
        }

        public async Task<UserEntity> Create(UserEntity user, string password)
        {
            try
            {
                await _userService.CreateUser(user);
                return user;
            } catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Delete(int id)
        {
            throw new NotImplementedException();
        }

        public UserEntity GetById(int id)
        {
            throw new NotImplementedException();
        }

        public void UpdateEmail(UserEntity user, string email = null)
        {
            throw new NotImplementedException();
        }

        public void UpdatePassword(UserEntity user, string password = null)
        {
            throw new NotImplementedException();
        }

        public void UpdatePhone(UserEntity user, string phone = null)
        {
            throw new NotImplementedException();
        }
    }
}
