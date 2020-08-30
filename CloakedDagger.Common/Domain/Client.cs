using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using CloakedDagger.Common.Enums;
using CloakedDagger.Common.Exceptions;
using CloakedDagger.Common.Utils;

namespace CloakedDagger.Common.Domain
{
    public class Client
    {
        public Guid Id { get; private set; }
        
        public string Name { get; private set; }
        
        public string Description { get; private set; }
        
        public string Secret { get; private set; }
        
        public bool Active { get; private set; }

        private readonly List<Identity> _allowedIdentities;

        public IEnumerable<Identity> AllowedIdentities => _allowedIdentities.AsReadOnly();

        private readonly List<ClientGrantType> _allowedGrantTypes;

        public IEnumerable<ClientGrantType> AllowedGrantTypes => _allowedGrantTypes.AsReadOnly();

        private readonly List<Scope> _allowedScopes;

        public IEnumerable<Scope> AllowedScopes => _allowedScopes.AsReadOnly();

        private readonly List<ClientUri> _uris;

        public IEnumerable<ClientUri> Uris => _uris.AsReadOnly();

        public Client()
        {
            _allowedIdentities = new List<Identity>();
            _allowedGrantTypes = new List<ClientGrantType>();
            _allowedScopes = new List<Scope>();
            _uris = new List<ClientUri>();
        }

        public static Client Create(string name, string description, string secret)
        {
            var client = new Client();

            client.Id = Guid.NewGuid();
            client.Name = name;
            client.Description = description;
            client.Secret = ShaUtils.Sha256HashString(secret);
            
            return client;
        }

        public void Rename(string name)
        {
            Name = name;
        }

        public void Redescribe(string description)
        {
            Description = description;
        }

        public void Activate()
        {
            if (string.IsNullOrEmpty(Secret))
            {
                throw new EntityValidationException("Secret must be provided before client can be activated!");
            }

            if (string.IsNullOrEmpty(Name))
            {
                throw new EntityValidationException("Name must be provided before client can be activated!");
            }

            if (_uris.All(u => u.Type != ClientUriType.Redirect))
            {
                throw new EntityValidationException("At least one Redirect URI must be provided before client can be activated!");
            }

            if (!_allowedGrantTypes.Any())
            {
                throw new EntityValidationException("At least one allowed grant type is must be provided before client can be activated!");
            }
            
            Active = true;
        }

        public void Deactivate()
        {
            Active = false;
        }
        

        public void AddAllowedIdentity(Identity identity)
        {
            _allowedIdentities.Add(identity);
        }

        public void RemoveAllowedIdentity(Identity identity)
        {
            _allowedIdentities.Remove(identity);
        }

        public void AddAllowedGrantType(ClientGrantType grantType)
        {
            _allowedGrantTypes.Add(grantType);
        }

        public void RemoveAllowedGrantType(ClientGrantType grantType)
        {
            _allowedGrantTypes.Remove(grantType);
        }

        public void AddUri(ClientUri uri)
        {
            _uris.Add(uri);
        }

        public void RemoveUri(ClientUri uri)
        {
            _uris.Remove(uri);
        }

        public void AddAllowedScope(Scope scope)
        {
            _allowedScopes.Add(scope);
        }

        public void RemoveAllowedScope(Scope scope)
        {
            _allowedScopes.Remove(scope);
        }
        
    }
}