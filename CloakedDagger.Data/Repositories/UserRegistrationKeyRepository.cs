using System;
using System.Linq;
using CloakedDagger.Common.Entities;
using CloakedDagger.Common.Repositories;
using OwlTin.Common;

namespace CloakedDagger.Data.Repositories
{
    public class UserRegistrationKeyRepository : IUserRegistrationKeyRepository
    {
        private readonly CloakedDaggerDbContext _db;

        public UserRegistrationKeyRepository(CloakedDaggerDbContext db)
        {
            this._db = db;
        }

        public UserRegistrationKeyEntity Get(Guid id)
        {
            return _db.UserRegistrationKeys.Active().FirstOrDefault(urk => urk.Id == id);
        }

        public UserRegistrationKeyEntity Get(string key)
        {
            return _db.UserRegistrationKeys.Active().FirstOrDefault(urk => urk.Key == key);
        }

        public void Create(UserRegistrationKeyEntity registrationKey)
        {
            _db.UserRegistrationKeys.Add(registrationKey);
            _db.SaveChanges();
        }

        public void Update(UserRegistrationKeyEntity registrationKey)
        {
            _db.UserRegistrationKeys.Attach(registrationKey);
            _db.SaveChanges();
        }
    }
}