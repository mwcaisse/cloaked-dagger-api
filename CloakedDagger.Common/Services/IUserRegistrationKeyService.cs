using System;
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
        
    }
}