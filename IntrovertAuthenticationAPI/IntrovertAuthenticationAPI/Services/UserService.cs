/* 
 * Author: Smailovic Alen
 * Date:   01.09.2019
*/

using IntrovertAuthenticationAPI.Entities;
using IntrovertAuthenticationAPI.Settings;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace IntrovertAuthenticationAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IMongoCollection<UserEntity> _userRepository;

        /// <summary>
        /// C-tor. Mongo DB connection.
        /// </summary>
        /// <param name="identityDbSettings"></param>
        public UserService(IdentityDatabase identityDbSettings)
        {
            if (identityDbSettings == null)
            {
                throw new ArgumentNullException(nameof(identityDbSettings), "Introvert Database invalid settings. Check appsettings file.");
            }

            if (String.IsNullOrWhiteSpace(identityDbSettings.ConnectionString) ||
                String.IsNullOrWhiteSpace(identityDbSettings.Server) ||
                String.IsNullOrWhiteSpace(identityDbSettings.Username) ||
                String.IsNullOrWhiteSpace(identityDbSettings.Password) ||
                String.IsNullOrWhiteSpace(identityDbSettings.Database))
            {
                throw new ArgumentNullException(nameof(identityDbSettings), "Introvert Database invalid property settings. Check appsettings file.");
            }

            try
            {
                string connectionString = identityDbSettings.ConnectionString;
                connectionString = connectionString.Replace("<server>", identityDbSettings.Server);
                connectionString = connectionString.Replace("<username>", identityDbSettings.Username);
                connectionString = connectionString.Replace("<password>", identityDbSettings.Password);

                MongoClient client = new MongoClient(connectionString);
                IMongoDatabase database = client.GetDatabase(identityDbSettings.Database);

                _userRepository = database.GetCollection<UserEntity>(nameof(UserEntity) + "Collection");
            }
            catch (Exception ex)
            {
                // log or manage the exception
                // TODO: create a logger
                throw ex;
            }
        }

        /// <summary>
        /// Create a user for the Application.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public async Task CreateUser(UserEntity user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "Cannot create an invalid UserEntity.");
            }

            try
            {
                user.UpdatedOn = DateTime.UtcNow;

                await _userRepository.InsertOneAsync(user);
            }
            catch (Exception ex)
            {
                // log or manage the exception
                // TODO: create a logger
                throw ex;
            }
        }

        /// <summary>
        /// Get user by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<UserEntity> GetUser(string id)
        {
            if (String.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id), "Cannot get user with an invalid ID.");
            }

            try
            {
                return await _userRepository.Find<UserEntity>(x => x.Id == id).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                // TODO: create a logger
                throw ex;
            }
        }

        /// <summary>
        /// Get all users as a list.
        /// </summary>
        /// <returns></returns>
        public async Task<List<UserEntity>> GetAllUsers()
        {
            try
            {
                return await _userRepository.Find<UserEntity>(x => true).ToListAsync();
            }
            catch (Exception ex)
            {
                // log or manage the exception
                // TODO: create a logger
                throw ex;
            }
        }

        /// <summary>
        /// Update user by id.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="newUser"></param>
        /// <returns></returns>
        public async Task UpdateUser(string id, UserEntity newUser)
        {
            if (String.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id), "Cannot update user with an invalid ID.");
            }

            if (newUser == null)
            {
                throw new ArgumentNullException(nameof(newUser), "Cannot update an invalid UserEntity.");
            }

            try
            {
                newUser.Id = id;
                newUser.UpdatedOn = DateTime.UtcNow;

                await _userRepository.ReplaceOneAsync<UserEntity>(x => x.Id == id, newUser);
            }
            catch (Exception ex)
            {
                // log or manage the exception
                // TODO: create a logger
                throw ex;
            }
        }

        /// <summary>
        /// Remove a user account.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task RemoveUser(string id)
        {
            if (String.IsNullOrWhiteSpace(id))
            {
                throw new ArgumentNullException(nameof(id), "Cannot remove user with an invalid ID.");
            }

            try
            {
                await _userRepository.DeleteOneAsync<UserEntity>(x => x.Id == id);
            }
            catch (Exception ex)
            {
                // log or manage the exception
                // TODO: create a logger
                throw ex;
            }
        }
    }
}
