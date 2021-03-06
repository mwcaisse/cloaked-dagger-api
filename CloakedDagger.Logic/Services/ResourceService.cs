using System;
using System.Collections;
using System.Collections.Generic;
using CloakedDagger.Common.Entities;
using CloakedDagger.Common.Exceptions;
using CloakedDagger.Common.Repositories;
using CloakedDagger.Common.Services;
using CloakedDagger.Common.ViewModels;

namespace CloakedDagger.Logic.Services
{
    public class ResourceService : IResourceService
    {
        private readonly IResourceRepository _resourceRepository;

        public ResourceService(IResourceRepository resourceRepository)
        {
            this._resourceRepository = resourceRepository;
        }
        
        public ResourceEntity Get(Guid id)
        {
            return _resourceRepository.Get(id);
        }

        public IEnumerable<ResourceEntity> GetAll()
        {
            return _resourceRepository.GetAll();
        }

        public ResourceEntity Create(ResourceViewModel resource)
        {
            resource.ResourceId = null; // clear this out, just in case
            ValidateResource(resource);
            
            var toCreate = new ResourceEntity()
            {
                Name = resource.Name,
                Description = resource.Description,
                Active = true
            };

            return _resourceRepository.Create(toCreate);
        }

        public ResourceEntity Update(ResourceViewModel resource)
        {
            if (!resource.ResourceId.HasValue)
            {
                throw new EntityValidationException("Must provide the ID of the resource to update!");
            }
            
            ValidateResource(resource);

            var existing = _resourceRepository.Get(resource.ResourceId.Value);
            existing.Name = resource.Name;
            existing.Description = resource.Description;

            _resourceRepository.Update(existing);
            return _resourceRepository.Get(existing.ResourceId);

        }

        public void Delete(Guid id)
        {
            _resourceRepository.Delete(id);
        }

        private void ValidateResource(ResourceViewModel resource)
        {
            ValidationUtils.ValidateViewModel(resource);

            if (_resourceRepository.ResourceWithNameExists(resource.Name, resource.ResourceId))
            {
                throw new EntityValidationException("A resource with this name already exists.");
            }
        }
    }
}