/* 
 * Author: Smailovic Alen
 * Date:   01.09.2019
*/

using IntrovertAuthenticationAPI.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IntrovertAuthenticationAPI.Services
{
    public interface IUserService
    {
        /// <summary>
        /// Create a user for the Application.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        Task CreateUser(UserEntity user);

        /// <summary>
        /// Get user by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<UserEntity> GetUser(string id);

        /// <summary>
        /// Get all users as a list.
        /// </summary>
        /// <returns></returns>
        Task<List<UserEntity>> GetAllUsers();

        /// <summary>
        /// Update user by id.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="newUser"></param>
        /// <returns></returns>
        Task UpdateUser(string id, UserEntity newUser);

        /// <summary>
        /// Remove a user account.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task RemoveUser(string id);
    }
}
