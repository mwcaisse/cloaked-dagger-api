using System;
using System.Linq;
using CloakedDagger.Common.Entities;
using CloakedDagger.Common.Repositories;
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
        
        public User Get(string username)
        {
            return _db.Users.Active().SingleOrDefault(u => u.Username == username);
        }

        public User Get(Guid id)
        {
            return _db.Users.Active().SingleOrDefault(u => u.UserId == id);
        }

        public bool UsernameExists(string username)
        {
            return _db.Users.Any(u => u.Username == username);
        }

        public void Create(User user)
        {
            _db.Users.Add(user);
            _db.SaveChanges();
        }
    }
}