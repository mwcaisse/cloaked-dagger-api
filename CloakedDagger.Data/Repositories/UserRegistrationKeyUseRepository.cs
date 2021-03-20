using CloakedDagger.Common.Entities;
using CloakedDagger.Common.Repositories;

namespace CloakedDagger.Data.Repositories
{
    public class UserRegistrationKeyUseRepository : IUserRegistrationKeyUseRepository
    {
        private readonly CloakedDaggerDbContext _db;

        public UserRegistrationKeyUseRepository(CloakedDaggerDbContext db)
        {
            this._db = db;
        }
        
        public void Create(UserRegistrationKeyUseEntity entity)
        {
            _db.UserRegistrationKeyUses.Add(entity);
            _db.SaveChanges();
        }
    }
}