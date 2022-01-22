using System;
using System.Collections.Generic;
using CloakedDagger.Common.Entities;

namespace CloakedDagger.Common.Repositories
{
    public interface IUserRegistrationKeyRepository
    {
        public UserRegistrationKeyEntity Get(Guid id);

        public IEnumerable<UserRegistrationKeyEntity> GetAll();
        
        public UserRegistrationKeyEntity Get(string key);

        /// <summary>
        ///  Returns true if a registration key with the given key already exists, false otherwise
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public bool Exists(string key);

        public void Create(UserRegistrationKeyEntity registrationKey);

        public void Update(UserRegistrationKeyEntity registrationKey);
    }
}