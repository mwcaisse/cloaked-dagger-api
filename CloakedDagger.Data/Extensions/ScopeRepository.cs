using System.Linq;
using CloakedDagger.Common.Entities;
using CloakedDagger.Common.Repositories;

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

        public Scope Get(string name)
        {
            return _db.Scopes.FirstOrDefault(s => s.Name == name);
        }

        public Scope Create(Scope scope)
        {
            _db.Scopes.Add(scope);
            _db.SaveChanges();
            return scope;
        }
    }
}