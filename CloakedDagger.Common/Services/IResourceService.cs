using System;
using System.Collections;
using System.Collections.Generic;
using CloakedDagger.Common.Entities;
using CloakedDagger.Common.ViewModels;

namespace CloakedDagger.Common.Services
{
    public interface IResourceService
    {

        public ResourceEntity Get(Guid id);

        public IEnumerable<ResourceEntity> GetAll();

        public ResourceEntity Create(ResourceViewModel resource);

        public ResourceEntity Update(ResourceViewModel resource);
        
        public void Delete(Guid id);

    }
}