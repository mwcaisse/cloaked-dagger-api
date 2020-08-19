using System;
using System.Collections.Generic;
using CloakedDagger.Common.Entities;

namespace CloakedDagger.Common.Repositories
{
    public interface IResourceScopeRepository
    {
        /// <summary>
        /// Fetches the resource scope with the given id
        /// </summary>
        /// <param name="id">The id of the resource scope</param>
        /// <returns>The resource scope with the given id, or null if non exists</returns>
        public ResourceScope Get(Guid id);

        /// <summary>
        /// Fetches the resource scopes associated with the given resource
        /// </summary>
        /// <param name="resourceId">The id of the resource</param>
        /// <returns>The list of scopes associated with the given resource, or empty list if none exist</returns>
        public IEnumerable<ResourceScope> GetForResource(Guid resourceId);

        /// <summary>
        /// Creates the given resource scope
        /// </summary>
        /// <param name="scope">The resource scope to create</param>
        /// <returns>The newly created resource scope</returns>
        public ResourceScope Create(ResourceScope scope);

        /// <summary>
        /// Deletes the resource scope with the given id
        /// </summary>
        /// <param name="id">The id of the resource scope to delete</param>
        public void Delete(Guid id);
    }
}