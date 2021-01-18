using System;

namespace CloakedDagger.Common.Repositories
{
    public interface IUserRoleRepository
    {

        /// <summary>
        ///  Creates the given mapping between user and role
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleId"></param>
        public void Create(Guid userId, Guid roleId);

        /// <summary>
        /// Deletes the given mapping between user and role
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="roleId"></param>
        public void Delete(Guid userId, Guid roleId);

    }
}