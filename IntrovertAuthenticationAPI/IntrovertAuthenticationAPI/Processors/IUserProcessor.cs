/* 
 * Author: Smailovic Alen
 * Date:   01.09.2019
*/

using IntrovertAuthenticationAPI.Entities;
using System.Threading.Tasks;

namespace IntrovertAuthenticationAPI.Processors
{
    public interface IUserProcessor
    {
        UserEntity Authenticate(string username, string password);
        UserEntity GetById(int id);
        Task<UserEntity> Create(UserEntity user, string password);
        void UpdatePassword(UserEntity user, string password = null);
        void UpdatePhone(UserEntity user, string phone = null);
        void UpdateEmail(UserEntity user, string email = null);
        void Delete(int id);
    }
}
