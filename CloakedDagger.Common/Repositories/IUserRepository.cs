using System;
using CloakedDagger.Common.Entities;

namespace CloakedDagger.Common.Repositories
{
    public interface IUserRepository
    {
        
        /// <summary>
        /// Retrieves the user with the given username
        /// </summary>
        /// <param name="username"></param>
        /// <returns>The user with that username, or null if no user exists</returns>
        public UserEntity Get(string username);

        /// <summary>
        /// Retrieves the user with the given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The user with the id, or null if no user exists</returns>
        public UserEntity Get(Guid id);

        /// <summary>
        /// Determines if a user with the given username exists or not.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public bool UsernameExists(string username);

        /// <summary>
        /// Creates the given user
        /// </summary>
        /// <param name="userEntity"></param>
        /// <returns></returns>
        public void Create(UserEntity userEntity);

        /// <summary>
        /// Updates the given user
        /// </summary>
        /// <param name="userEntity"></param>
        public void Update(UserEntity userEntity);

        /// <summary>
        /// Delete the user with the given id
        /// </summary>
        /// <param name="id"></param>
        public void Delete(Guid id);

    }
}