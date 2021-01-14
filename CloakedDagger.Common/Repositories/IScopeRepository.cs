using System.Collections.Generic;
using CloakedDagger.Common.Entities;

namespace CloakedDagger.Common.Repositories
{
    public interface IScopeRepository
    {
        
        /// <summary>
        /// Determines if a scope with the given name exists
        /// </summary>
        /// <param name="name">The name of the scope</param>
        /// <returns>True if a scope with the given name exists, false otherwise</returns>
        public bool Exists(string name);

        /// <summary>
        /// Fetches a scope with the given name
        /// </summary>
        /// <param name="name">The name of the scope</param>
        /// <returns>The scope if a scope with the given name exists, null otherwise</returns>
        public ScopeEntity Get(string name);

        /// <summary>
        /// Fetches a list of all active scopes
        /// </summary>
        /// <returns></returns>
        public IEnumerable<ScopeEntity> GetAll();

        /// <summary>
        /// Fetches a list of scopes that match the given names
        /// </summary>
        /// <param name="names"></param>
        /// <returns></returns>
        public IEnumerable<ScopeEntity> GetWithNames(IEnumerable<string> names);
        
        /// <summary>
        /// Creates the given scope
        /// </summary>
        /// <param name="scopeEntity">the scope to create</param>
        /// <returns>The newly created scope</returns>
        public ScopeEntity Create(ScopeEntity scopeEntity);
    }
}