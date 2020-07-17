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
        
    }
}