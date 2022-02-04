using System;
using System.Collections.Generic;
using CloakedDagger.Common.Entities;

namespace CloakedDagger.Common.Repositories
{
    public interface IUserEmailVerificationRequestRepository
    {
        public UserEmailVerificationRequestEntity GetActiveByKey(string key);

        public IEnumerable<UserEmailVerificationRequestEntity> GetActiveForUser(Guid userId);

        public void Create(UserEmailVerificationRequestEntity entity);

        public void Update(UserEmailVerificationRequestEntity entity);
    }
}