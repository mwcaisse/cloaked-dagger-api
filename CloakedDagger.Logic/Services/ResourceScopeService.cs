using System;
using System.Collections.Generic;
using CloakedDagger.Common.Entities;
using CloakedDagger.Common.Repositories;
using CloakedDagger.Common.ViewModels;
using CloakedDagger.Logic;
using OwlTin.Common.Exceptions;

namespace CloakedDagger.Common.Services
{
    public class ResourceScopeService : IResourceScopeService
    {
        private readonly IResourceScopeRepository _resourceScopeRepository;

        private readonly IResourceRepository _resourceRepository;
        
        private readonly IScopeRepository _scopeRepository;

        public ResourceScopeService(IResourceScopeRepository resourceScopeRepository, 
            IResourceRepository resourceRepository, IScopeRepository scopeRepository)
        {
            this._resourceScopeRepository = resourceScopeRepository;
            this._resourceRepository = resourceRepository;
            this._scopeRepository = scopeRepository;
        }
        
        public ResourceScope Get(Guid id)
        {
            return _resourceScopeRepository.Get(id);
        }

        public IEnumerable<ResourceScope> GetForResource(Guid resourceId)
        {
            return _resourceScopeRepository.GetForResource(resourceId);
        }

        public ResourceScope Create(AddResourceScopeViewModel vm)
        {
            ValidationUtils.ValidateViewModel(vm);

            if (!_resourceRepository.Exists(vm.ResourceId))
            {
                throw new EntityValidationException("No resources with the given ID exists!");
            }

            var scope = _scopeRepository.Get(vm.ScopeName);
            if (null == scope)
            {
                scope = new Scope()
                {
                    Name = vm.ScopeName,
                    Description = vm.Description
                };
                scope = _scopeRepository.Create(scope);
            }

            var resourceScope = new ResourceScope()
            {
                ResourceId = vm.ResourceId,
                Scope = scope
            };

            return _resourceScopeRepository.Create(resourceScope);
        }

        public void Delete(Guid id)
        {
            _resourceScopeRepository.Delete(id);
        }
    }
}