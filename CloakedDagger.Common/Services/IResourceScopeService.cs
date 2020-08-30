using System;
using System.Collections.Generic;
using CloakedDagger.Common.Entities;
using CloakedDagger.Common.ViewModels;

namespace CloakedDagger.Common.Services
{
    public interface IResourceScopeService
    {

        public ResourceScopeEntity Get(Guid id);
        
        public IEnumerable<ResourceScopeEntity> GetForResource(Guid resourceId);

        public ResourceScopeEntity Create(AddResourceScopeViewModel scope);

        public void Delete(Guid id);

    }
}