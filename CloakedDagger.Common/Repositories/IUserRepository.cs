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
        public User Get(string username);

        /// <summary>
        /// Determines if a user with the given username exists or not.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public bool UsernameExists(string username);

        /// <summary>
        /// Creates the given user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public void Create(User user);

    }
}