using System;
using System.Collections.Generic;
using System.Linq;
using CloakedDagger.Common.Entities;
using CloakedDagger.Common.Repositories;
using CloakedDagger.Data.Extensions;
using OwlTin.Common;

namespace CloakedDagger.Data.Repositories
{
    public class ResourceRepository : IResourceRepository
    {
        private readonly CloakedDaggerDbContext _db;

        public ResourceRepository(CloakedDaggerDbContext db)
        {
            this._db = db;
        }
        
        public ResourceEntity Get(Guid id)
        {
            return _db.Resources.Build().FirstOrDefault(r => r.ResourceId == id);
        }

        public bool Exists(Guid id)
        {
            return _db.Resources.Any(r => r.ResourceId == id);
        }

        public IEnumerable<ResourceEntity> GetAll()
        {
            return _db.Resources.Build();
        }

        public ResourceEntity Create(ResourceEntity resourceEntity)
        {
            _db.Resources.Add(resourceEntity);
            _db.SaveChanges();

            return resourceEntity;
        }

        public ResourceEntity Update(ResourceEntity resourceEntity)
        {
            _db.Resources.Attach(resourceEntity);
            _db.SaveChanges();

            return resourceEntity;
        }

        public void Delete(Guid id)
        {
            var resource = _db.Resources.FirstOrDefault(r => r.ResourceId == id);
            if (null != resource)
            {
                _db.Resources.Remove(resource);
                _db.SaveChanges();
            }
        }

        public bool ResourceWithNameExists(string name, Guid? exclude = null)
        {
            var query = _db.Resources.Where(r => r.Name == name);
            if (exclude.HasValue)
            {
                query = query.Where(r => r.ResourceId != exclude);
            }

            return query.Any();
        }
    }
}