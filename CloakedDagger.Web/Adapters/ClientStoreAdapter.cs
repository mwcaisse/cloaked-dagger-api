using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloakedDagger.Common.Enums;
using CloakedDagger.Common.Services;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Stores;

namespace CloakedDagger.Web.Adapters
{
    public class ClientStoreAdapter : IClientStore
    {
        private readonly IClientService _clientService;

        public ClientStoreAdapter(IClientService clientService)
        {
            this._clientService = clientService;
        }
        
        public async Task<Client> FindClientByIdAsync(string clientId)
        {
            var client = _clientService.Get(Guid.Parse(clientId));

            return new Client()
            {
                ClientId = client.Id.ToString(),
                ClientName = client.Name,
                Description =  client.Description,
                
                AllowedGrantTypes = client.AllowedGrantTypes.Select(agt => _allowedGrantTypesMapping[agt]).ToList(),
                AllowOfflineAccess = false,
                ClientSecrets =
                {
                    new Secret(client.Secret)
                },
                RedirectUris = client.Uris
                    .Where(u => u.Type == ClientUriType.Redirect)
                    .Select(u => u.Uri)
                    .ToList(),
                PostLogoutRedirectUris = client.Uris
                    .Where(u => u.Type == ClientUriType.PostLogoutRedirect)
                    .Select(u => u.Uri)
                    .ToList(),
                
                AllowedScopes = client.AllowedScopes
                    .Union(client.AllowedIdentities.Select(ai => _allowedIdentityMapping[ai]))
                    .Union(new List<string>()
                    {
                        "id",
                        "username",
                        "name"
                    })
                    .ToList(),
                
                Enabled = true
            };
        }

        private readonly Dictionary<ClientGrantType, string> _allowedGrantTypesMapping =
            new Dictionary<ClientGrantType, string>()
            {
                {ClientGrantType.AuthorizationCode, GrantType.AuthorizationCode},
                {ClientGrantType.ClientCredentials, GrantType.ClientCredentials}
            };

        private readonly Dictionary<Identity, string> _allowedIdentityMapping = new Dictionary<Identity, string>()
        {
            {Identity.OpenId, IdentityServerConstants.StandardScopes.OpenId},
            {Identity.Profile, IdentityServerConstants.StandardScopes.Profile},
            {Identity.Email, IdentityServerConstants.StandardScopes.Email},
            {Identity.User, UserIdentityResource.Scope}
        };
    }
}