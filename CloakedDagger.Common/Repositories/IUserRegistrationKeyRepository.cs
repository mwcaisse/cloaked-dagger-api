using System;
using CloakedDagger.Common.Entities;

namespace CloakedDagger.Common.Repositories
{
    public interface IUserRegistrationKeyRepository
    {
        public UserRegistrationKeyEntity Get(Guid id);
        
        public UserRegistrationKeyEntity Get(string key);

        public void Create(UserRegistrationKeyEntity registrationKey);

        public void Update(UserRegistrationKeyEntity registrationKey);
    }
}