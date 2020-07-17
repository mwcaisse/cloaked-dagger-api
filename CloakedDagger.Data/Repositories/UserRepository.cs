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
    }
}