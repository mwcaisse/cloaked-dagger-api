using System;
using System.Collections;
using System.Collections.Generic;
using CloakedDagger.Common.Entities;

namespace CloakedDagger.Common.Repositories
{
    public interface IResourceRepository
    {
        public ResourceEntity Get(Guid id);

        /// <summary>
        /// Determines if a resource with the given id exists
        /// </summary>
        /// <param name="id">The id of the resource</param>
        /// <returns>True if the resource exists, false otherwise</returns>
        public bool Exists(Guid id);

        public IEnumerable<ResourceEntity> GetAll();

        /// <summary>
        /// Fetches the Resources that name is contained in names
        /// </summary>
        /// <param name="names"></param>
        /// <returns></returns>
        public IEnumerable<ResourceEntity> GetWithNames(IEnumerable<string> names);

        /// <summary>
        /// Fetches the Resources that allow at least one of the given scope names
        /// </summary>
        /// <param name="scopeNames"></param>
        /// <returns></returns>
        public IEnumerable<ResourceEntity> GetByScopes(IEnumerable<string> scopeNames);

        public ResourceEntity Create(ResourceEntity resourceEntity);

        public ResourceEntity Update(ResourceEntity resourceEntity);

        public void Delete(Guid id);

        public bool ResourceWithNameExists(string name, Guid? exclude = null);
    }
}