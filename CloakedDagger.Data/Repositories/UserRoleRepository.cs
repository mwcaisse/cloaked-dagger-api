using System;
using System.Linq;
using CloakedDagger.Common.Entities;
using CloakedDagger.Common.Repositories;

namespace CloakedDagger.Data.Repositories
{
    public class UserRoleRepository : IUserRoleRepository
    {
        private readonly CloakedDaggerDbContext _db;

        public UserRoleRepository(CloakedDaggerDbContext db)
        {
            this._db = db;
        }
        
        public void Create(Guid userId, Guid roleId)
        {
            if (_db.UserRoles.Any(ur => ur.UserId == userId && ur.RoleId == roleId))
            {
                // This user / role mapping already exists, don't do anything.
                return;
            }
            _db.UserRoles.Add(new UserRoleEntity()
            {
                UserId = userId,
                RoleId = roleId
            });
            _db.SaveChanges();
        }

        public void Delete(Guid userId, Guid roleId)
        {
            var toDelete = _db.UserRoles
                .FirstOrDefault(ur => ur.UserId == userId && ur.RoleId == roleId);

            if (null != toDelete)
            {
                _db.UserRoles.Remove(toDelete);
                _db.SaveChanges();
            }
        }
    }
}