using System;
using System.Collections;
using System.Collections.Generic;
using CloakedDagger.Common.Entities;

namespace CloakedDagger.Common.Repositories
{
    public interface IResourceRepository
    {
        public Resource Get(Guid id);

        /// <summary>
        /// Determines if a resource with the given id exists
        /// </summary>
        /// <param name="id">The id of the resource</param>
        /// <returns>True if the resource exists, false otherwise</returns>
        public bool Exists(Guid id);

        public IEnumerable<Resource> GetAll();

        public Resource Create(Resource resource);

        public Resource Update(Resource resource);

        public void Delete(Guid id);

        public bool ResourceWithNameExists(string name, Guid? exclude = null);
    }
}