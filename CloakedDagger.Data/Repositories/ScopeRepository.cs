using System.Collections.Generic;
using System.Linq;
using CloakedDagger.Common.Entities;
using CloakedDagger.Common.Repositories;
using OwlTin.Common;

namespace CloakedDagger.Data.Extensions
{
    public class ScopeRepository : IScopeRepository
    {
        private readonly CloakedDaggerDbContext _db;

        public ScopeRepository(CloakedDaggerDbContext db)
        {
            this._db = db;
        }
        
        public bool Exists(string name)
        {
            return _db.Scopes.Any(s => s.Name == name);
        }

        public ScopeEntity Get(string name)
        {
            return _db.Scopes.FirstOrDefault(s => s.Name == name);
        }

        public IEnumerable<ScopeEntity> GetAll()
        {
            return _db.Scopes.Active();
        }

        public IEnumerable<ScopeEntity> GetWithNames(IEnumerable<string> names)
        {
            return _db.Scopes.Active().Where(s => names.Contains(s.Name));
        }

        public ScopeEntity Create(ScopeEntity scopeEntity)
        {
            _db.Scopes.Add(scopeEntity);
            _db.SaveChanges();
            return scopeEntity;
        }
    }
}