using System;
using CloakedDagger.Common.Entities;

namespace CloakedDagger.Common.Repositories
{
    public interface IRoleRepository
    {
        /// <summary>
        /// Fetches the role with the given id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>The role with the id, or null if none exists</returns>
        public RoleEntity Get(Guid id);

        /// <summary>
        /// Fetches the role with the given name
        /// </summary>
        /// <param name="name"></param>
        /// <returns>The role with that name, or null if none exists</returns>
        public RoleEntity Get(string name);

        /// <summary>
        /// Determines if a role with the given name exists or not.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>True if the role exists, false otherwise</returns>
        public bool RoleExists(string name);
        
        /// <summary>
        /// Creates the given role
        /// </summary>
        /// <param name="roleEntity"></param>
        public void Create(RoleEntity roleEntity);

    }
}