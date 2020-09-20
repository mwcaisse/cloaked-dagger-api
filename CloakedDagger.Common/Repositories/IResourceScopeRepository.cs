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
        public ResourceScopeEntity Get(Guid id);

        /// <summary>
        /// Fetches the resource scopes associated with the given resource
        /// </summary>
        /// <param name="resourceId">The id of the resource</param>
        /// <returns>The list of scopes associated with the given resource, or empty list if none exist</returns>
        public IEnumerable<ResourceScopeEntity> GetForResource(Guid resourceId);

        /// <summary>
        /// Determines if the scope with the given id exists on the given resource
        /// </summary>
        /// <param name="resourceId">The id of the resource</param>
        /// <param name="scopeId">The id of the scope</param>
        /// <returns>True if the scope exists on the resource, false otherwise</returns>
        public bool ExistsOnResource(Guid resourceId, Guid scopeId);

        /// <summary>
        /// Determines if the scope with the given name exists on any resource(s)
        /// </summary>
        /// <param name="scopeName">The name of the scope to check</param>
        /// <returns>True if at least one resource defines the scope, false otherwise</returns>
        public bool ExistsOnAnyResource(string scopeName);

        public IEnumerable<ScopeEntity> Search(string text);

        /// <summary>
        /// Creates the given resource scope
        /// </summary>
        /// <param name="scopeEntity">The resource scope to create</param>
        /// <returns>The newly created resource scope</returns>
        public ResourceScopeEntity Create(ResourceScopeEntity scopeEntity);

        /// <summary>
        /// Deletes the resource scope with the given id
        /// </summary>
        /// <param name="id">The id of the resource scope to delete</param>
        public void Delete(Guid id);
    }
}