using System;
using System.Collections.Generic;
using System.Linq;
using CloakedDagger.Common.Entities;
using CloakedDagger.Common.Repositories;
using OwlTin.Common;

namespace CloakedDagger.Data.Repositories
{
    public class UserEmailVerificationRequestRepository : IUserEmailVerificationRequestRepository
    {
        private readonly CloakedDaggerDbContext _db;

        public UserEmailVerificationRequestRepository(CloakedDaggerDbContext db)
        {
            _db = db;
        }

        public UserEmailVerificationRequestEntity GetActiveByKey(string key)
        {
            return _db.UserEmailVerificationRequests.Active().FirstOrDefault(x => x.ValidationKey == key);
        }

        public IEnumerable<UserEmailVerificationRequestEntity> GetActiveForUser(Guid userId)
        {
            return _db.UserEmailVerificationRequests.Active().Where(x => x.UserId == userId);
        }

        public void Create(UserEmailVerificationRequestEntity entity)
        {
            _db.UserEmailVerificationRequests.Add(entity);
            _db.SaveChanges();
        }

        public void Update(UserEmailVerificationRequestEntity entity)
        {
            _db.UserEmailVerificationRequests.Attach(entity);
            _db.SaveChanges();
        }
    }
}