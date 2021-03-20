using System;
using System.Linq;
using CloakedDagger.Common.Entities;
using CloakedDagger.Common.Repositories;
using CloakedDagger.Data.Extensions;
using Microsoft.EntityFrameworkCore;
using OwlTin.Common;

namespace CloakedDagger.Data.Repositories
{
    public class UserRepository : IUserRepository
    {

        private readonly CloakedDaggerDbContext _db;

        public UserRepository(CloakedDaggerDbContext db)
        {
            this._db = db;
        }
        
        public UserEntity Get(string username)
        {
            return _db.Users.Active().Build().SingleOrDefault(u => u.Username == username);
        }

        public UserEntity Get(Guid id)
        {
            return _db.Users
                .Active()
                .Build()
                .SingleOrDefault(u => u.UserId == id);
        }

        public bool UsernameExists(string username)
        {
            return _db.Users.Any(u => u.Username == username);
        }

        public void Create(UserEntity userEntity)
        {
            _db.Users.Add(userEntity);
            _db.SaveChanges();
        }

        public void Delete(Guid id)
        {
            var toDelete = Get(id);
            if (null != toDelete)
            {
                _db.Users.Remove(toDelete);
            }
        }
    }
}