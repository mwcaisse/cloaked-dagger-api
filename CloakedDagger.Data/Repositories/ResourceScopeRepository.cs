using System;
using System.Collections.Generic;
using System.Linq;
using CloakedDagger.Common.Entities;
using CloakedDagger.Common.Repositories;
using CloakedDagger.Data.Extensions;

namespace CloakedDagger.Data.Repositories
{
    public class ResourceScopeRepository : IResourceScopeRepository
    {
        
        private readonly CloakedDaggerDbContext _db;

        public ResourceScopeRepository(CloakedDaggerDbContext db)
        {
            this._db = db;
        }
        public ResourceScope Get(Guid id)
        {
            return _db.ResourceScopes.Build().FirstOrDefault(rs => rs.ResourceScopeId == id);
        }

        public IEnumerable<ResourceScope> GetForResource(Guid resourceId)
        {
            return _db.ResourceScopes.Build().Where(rs => rs.ResourceId == resourceId);
        }

        public bool ExistsOnResource(Guid resourceId, Guid scopeId)
        {
            return _db.ResourceScopes.Any(rs => rs.ResourceId == resourceId && rs.ScopeId == scopeId);
        }

        public ResourceScope Create(ResourceScope scope)
        {
            _db.ResourceScopes.Add(scope);
            _db.SaveChanges();
            return scope;
        }

        public void Delete(Guid id)
        {
            var toRemove = _db.ResourceScopes.FirstOrDefault(rs => rs.ResourceScopeId == id);
            if (null != toRemove)
            {
                _db.Remove(toRemove);
                _db.SaveChanges();
            }
        }
    }
}