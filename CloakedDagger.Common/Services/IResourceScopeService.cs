using System;
using System.Collections.Generic;
using CloakedDagger.Common.Entities;
using CloakedDagger.Common.ViewModels;

namespace CloakedDagger.Common.Services
{
    public interface IResourceScopeService
    {

        public ResourceScope Get(Guid id);
        
        public IEnumerable<ResourceScope> GetForResource(Guid resourceId);

        public ResourceScope Create(AddResourceScopeViewModel scope);

        public void Delete(Guid id);

    }
}