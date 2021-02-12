using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloakedDagger.Common.Entities;
using CloakedDagger.Common.Enums;
using CloakedDagger.Common.Repositories;
using CloakedDagger.Common.Services;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Stores;

namespace CloakedDagger.Web.Adapters
{
    public class ResourceStoreAdapter : IResourceStore
    {

        private readonly IResourceRepository _resourceRepository;
        private readonly IScopeRepository _scopeRepository;
        
        private readonly List<IdentityResource> _idenities = new List<IdentityResource>()
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Email(),
            new IdentityResources.Profile(),
            new UserIdentityResource()
        };

        public ResourceStoreAdapter(IResourceRepository resourceRepository, IScopeRepository scopeRepository)
        {
            this._resourceRepository = resourceRepository;
            this._scopeRepository = scopeRepository;
        }
        
        public async Task<IEnumerable<IdentityResource>> FindIdentityResourcesByScopeNameAsync(IEnumerable<string> scopeNames)
        {
            return _idenities.Where(i => scopeNames.Contains(i.Name)).ToList();
        }

        public async Task<IEnumerable<ApiScope>> FindApiScopesByNameAsync(IEnumerable<string> scopeNames)
        {
            return _scopeRepository.GetWithNames(scopeNames).Select(ScopeToApiScope).ToList();
        }

        public async Task<IEnumerable<ApiResource>> FindApiResourcesByScopeNameAsync(IEnumerable<string> scopeNames)
        {
            return _resourceRepository.GetByScopes(scopeNames).Select(ResourceToApiResource).ToList();
        }

        public async Task<IEnumerable<ApiResource>> FindApiResourcesByNameAsync(IEnumerable<string> apiResourceNames)
        {
            return _resourceRepository.GetWithNames(apiResourceNames).Select(ResourceToApiResource).ToList();
        }

        public async Task<Resources> GetAllResourcesAsync()
        {
            var apis = _resourceRepository.GetAll()
                .Where(r => r.Active)
                .Select(ResourceToApiResource)
                .ToHashSet();
            
            var scopes = _scopeRepository.GetAll()
                .Select(ScopeToApiScope)
                .ToList();
                
            return new Resources(null, apis, scopes);

        }

        protected ApiScope ScopeToApiScope(ScopeEntity scope)
        {
            return new ApiScope()
            {
                Name = scope.Name,
                Enabled = true,
                Description = scope.Description,
                DisplayName = scope.Name
            };
        }
        

        protected ApiResource ResourceToApiResource(ResourceEntity resource)
        {
            return new ApiResource()
            {
                Name = resource.Name,
                DisplayName =  resource.Name,
                Scopes = resource.AvailableScopes.Select(s => s.ScopeEntity.Name).ToHashSet(),
                Description = resource.Description,
                Enabled = resource.Active
            };
        }
    }
}