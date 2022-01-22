using System;
using System.Collections.Generic;
using CloakedDagger.Common.Entities;

namespace CloakedDagger.Common.Services
{
    public interface IUserRegistrationKeyService
    {
        /// <summary>
        ///  Determines if the registration key with the given key value is valid for another use
        /// </summary>
        /// <param name="key">The key value of the registration key</param>
        /// <returns></returns>
        public bool IsValid(string key);

        /// <summary>
        /// Use the registration key with the given key value
        /// </summary>
        /// <param name="key">The key value of the registration key</param>
        /// <returns>True if valid and used successfully, false otherwise</returns>
        public bool Use(string key, Guid userId);

        /// <summary>
        /// Returns all User Registration Keys
        /// </summary>
        /// <returns>All user registration keys</returns>
        public IEnumerable<UserRegistrationKeyEntity> GetAll();

        /// <summary>
        /// Creates a new registration key with the given key value and number of uses
        /// </summary>
        /// <param name="key">The key value for the registration key</param>
        /// <param name="uses">The initial number of uses for the key</param>
        /// <returns>The newly created registration key</returns>
        public UserRegistrationKeyEntity Create(string key, int uses);

    }
}