using System;
using System.Linq;
using CloakedDagger.Common.Entities;
using CloakedDagger.Common.Repositories;

namespace CloakedDagger.Data.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly CloakedDaggerDbContext _db;

        public RoleRepository(CloakedDaggerDbContext db)
        {
            this._db = db;
        }
        
        public RoleEntity Get(Guid id)
        {
            return _db.Roles.FirstOrDefault(r => r.RoleId == id);
        }

        public RoleEntity Get(string name)
        {
            return _db.Roles.FirstOrDefault(r => r.Name == name);
        }

        public bool RoleExists(string name)
        {
            return null != Get(name);
        }

        public void Create(RoleEntity roleEntity)
        {
            _db.Roles.Add(roleEntity);
            _db.SaveChanges();
        }
    }
}