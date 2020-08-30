using System;
using System.Collections.Generic;
using CloakedDagger.Common.Entities;
using CloakedDagger.Common.Exceptions;
using CloakedDagger.Common.Repositories;
using CloakedDagger.Common.Services;
using CloakedDagger.Common.ViewModels;


namespace CloakedDagger.Logic.Services
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
        
        public ResourceScopeEntity Get(Guid id)
        {
            return _resourceScopeRepository.Get(id);
        }

        public IEnumerable<ResourceScopeEntity> GetForResource(Guid resourceId)
        {
            return _resourceScopeRepository.GetForResource(resourceId);
        }

        public ResourceScopeEntity Create(AddResourceScopeViewModel vm)
        {
            ValidationUtils.ValidateViewModel(vm);

            if (!_resourceRepository.Exists(vm.ResourceId))
            {
                throw new EntityValidationException("No resources with the given ID exists!");
            }

            var scope = _scopeRepository.Get(vm.Name);
            if (null == scope)
            {
                scope = new ScopeEntity()
                {
                    Name = vm.Name,
                    Description = vm.Description
                };
                scope = _scopeRepository.Create(scope);
            }
            else
            {
                if (_resourceScopeRepository.ExistsOnResource(vm.ResourceId, scope.ScopeId))
                {
                    throw new EntityValidationException("This scope already exists on this resource!");
                }
            }

            var resourceScope = new ResourceScopeEntity()
            {
                ResourceId = vm.ResourceId,
                ScopeEntity = scope
            };

            return _resourceScopeRepository.Create(resourceScope);
        }

        public void Delete(Guid id)
        {
            _resourceScopeRepository.Delete(id);
        }
    }
}