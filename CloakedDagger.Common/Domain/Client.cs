using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Security.Cryptography;
using CloakedDagger.Common.Domain.Events.Client;
using CloakedDagger.Common.Enums;
using CloakedDagger.Common.Exceptions;
using CloakedDagger.Common.Utils;
using Microsoft.EntityFrameworkCore.Internal;

namespace CloakedDagger.Common.Domain
{
    public class Client : IDomainModel
    {
        public Guid Id { get; }
        
        public string Key => Id.ToString();
        
        public string Name { get; private set; }
        
        public string Description { get; private set; }
        
        public string Secret { get; private set; }
        
        public bool Active { get; private set; }

        private readonly ISet<Identity> _allowedIdentities;

        public IEnumerable<Identity> AllowedIdentities => _allowedIdentities.ToImmutableHashSet();

        private readonly ISet<ClientGrantType> _allowedGrantTypes;

        public IEnumerable<ClientGrantType> AllowedGrantTypes => _allowedGrantTypes.ToImmutableHashSet();

        private readonly List<string> _allowedScopes;

        public IEnumerable<string> AllowedScopes => _allowedScopes.AsReadOnly();

        private readonly List<ClientUri> _uris;

        public IEnumerable<ClientUri> Uris => _uris.AsReadOnly();

        private readonly List<ClientDomainEvent> _changes;
        public IEnumerable<ClientDomainEvent> Changes => _changes.AsReadOnly();

        public static class EventHandler {

            public static Client Hydrate(Guid clientId, IEnumerable<ClientDomainEvent> events)
            {
                var client = new Client(clientId);

                events.Aggregate(client, (c, cde) =>
                {
                    switch (cde)
                    {
                        case ClientCreatedEvent e:
                            return Created(c, e);
                        case ClientActivatedEvent e:
                            return Activated(c, e);
                        case ClientDeactivatedEvent e:
                            return Deactivated(c, e);
                        case ClientRenamedEvent e:
                            return Renamed(c, e);
                        case ClientRedescribedEvent e:
                            return Redescribed(c, e);
                        case AddedClientUriEvent e:
                            return UriAdded(c, e);
                        case ModifiedClientUriEvent e:
                            return UriModified(c, e);
                        case RemovedClientUriEvent e:
                            return UriRemoved(c, e);
                        case AddedAllowedIdentityEvent e:
                            return AllowedIdentityAdded(c, e);
                        case RemovedAllowedIdentityEvent e:
                            return AllowedIdentityRemoved(c, e);
                        case AddedAllowedGrantTypeEvent e:
                            return AllowedGrantTypeAdded(c, e);
                        case RemovedAllowedGrantTypeEvent e:
                            return AllowedGrantTypeRemoved(c, e);
                        case AddedAllowedScopeEvent e:
                            return AllowedScopeAdded(c, e);
                        case RemovedAllowedScopeEvent e:
                            return AllowedScopeRemoved(c, e);
                        case ClientIgnoredEvent e:
                            return c;
                        default:
                            throw new Exception($"Could not re-hydrate client. Unknown event {cde.Type}");
                    }
                });

                return client;
            }

            public static Client Created(Client client, ClientCreatedEvent e)
            {
                client.Name = e.Name;
                client.Description = e.Description;
                client.Secret = e.Secret;
                client.Active = false;

                return client;
            }
            public static Client Activated(Client client, ClientActivatedEvent e)
            {
                client.Active = true;
                return client;
            }

            public static Client Deactivated(Client client, ClientDeactivatedEvent e)
            {
                client.Active = false;
                return client;
            }

            public static Client Renamed(Client client, ClientRenamedEvent e)
            {
                client.Name = e.Name;
                return client;
            }

            public static Client Redescribed(Client client, ClientRedescribedEvent e)
            {
                client.Description = e.Description;
                return client;
            }

            public static Client UriAdded(Client client, AddedClientUriEvent e)
            {
                var uri = new ClientUri()
                {
                    Id = e.ClientUriId,
                    Type = e.UriType,
                    Uri = e.Uri
                };
                client._uris.Add(uri);
                
                return client;
            }

            public static Client UriModified(Client client, ModifiedClientUriEvent e)
            {
                var modified = client._uris.First(u => u.Id == e.ClientUriId);
                
                modified.Type = e.UriType;
                modified.Uri = e.Uri;
                
                return client;
            }

            public static Client UriRemoved(Client client, RemovedClientUriEvent e)
            {
                client._uris.RemoveAll(u => u.Id == e.ClientUriId);
                
                return client;
            }

            public static Client AllowedGrantTypeAdded(Client client, AddedAllowedGrantTypeEvent e)
            {
                client._allowedGrantTypes.Add(e.GrantType);
                return client;
            }

            public static Client AllowedGrantTypeRemoved(Client client, RemovedAllowedGrantTypeEvent e)
            {
                client._allowedGrantTypes.Remove(e.GrantType);
                return client;
            }

            public static Client AllowedIdentityAdded(Client client, AddedAllowedIdentityEvent e)
            {
                client._allowedIdentities.Add(e.Identity);
                return client;
            }

            public static Client AllowedIdentityRemoved(Client client, RemovedAllowedIdentityEvent e)
            {
                client._allowedIdentities.Remove(e.Identity);
                return client;
            }

            public static Client AllowedScopeAdded(Client client, AddedAllowedScopeEvent e)
            {
                client._allowedScopes.Add(e.ScopeName);
                return client;
            }

            public static Client AllowedScopeRemoved(Client client, RemovedAllowedScopeEvent e)
            {
                client._allowedScopes.Remove(e.ScopeName);
                return client;
            }
        }

        private Client(Guid id)
        {
            Id = id;
            
            _allowedIdentities = new HashSet<Identity>();
            _allowedGrantTypes = new HashSet<ClientGrantType>();
            _allowedScopes = new List<string>();
            _uris = new List<ClientUri>();
            _changes = new List<ClientDomainEvent>();
        }
        
        public Client(string name, string secret, string description = null) : this (Guid.NewGuid())
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new EntityValidationException("Name must not be blank!");
            }

            if (string.IsNullOrWhiteSpace(secret))
            {
                throw new EntityValidationException("Secret must not be blank!");
            }

            var e = new ClientCreatedEvent()
            {
                ClientId = Id,
                Name = name,
                Secret = ShaUtils.Sha256HashString(secret),
                Description = description,
                OccurredOn = DateTime.UtcNow
            };
            _changes.Add(e);
            EventHandler.Created(this, e);
        }

        public void Rename(string name)
        {
            var e = new ClientRenamedEvent()
            {
                ClientId = Id,
                Name = name,
                OccurredOn = DateTime.UtcNow
            };
            _changes.Add(e);
            EventHandler.Renamed(this, e);
        }

        public void Redescribe(string description)
        {
            var e = new ClientRedescribedEvent()
            {
                ClientId = Id,
                Description = description,
                OccurredOn = DateTime.UtcNow
            };
            _changes.Add(e);
            EventHandler.Redescribed(this, e);
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

            var e = new ClientActivatedEvent()
            {
                ClientId = Id,
                OccurredOn = DateTime.UtcNow
            };
            _changes.Add(e);
            EventHandler.Activated(this, e);
        }

        public void Deactivate()
        {
            var e = new ClientDeactivatedEvent()
            {
                ClientId = Id,
                OccurredOn = DateTime.UtcNow
            };
            _changes.Add(e);
            EventHandler.Deactivated(this, e);
        }
        

        public void AddAllowedIdentity(Identity identity)
        {
            if (_allowedIdentities.Contains(identity))
            {
                throw new EntityValidationException("This Identity is already allowed.");
            }

            var e = new AddedAllowedIdentityEvent()
            {
                Identity = identity,
                ClientId = Id,
                OccurredOn = DateTime.UtcNow
            };
            _changes.Add(e);
            EventHandler.AllowedIdentityAdded(this, e);
        }

        public void RemoveAllowedIdentity(Identity identity)
        {
            if (!_allowedIdentities.Contains(identity))
            {
                throw new EntityValidationException("This identity is not currently allowed.");
            }
            
            var e = new RemovedAllowedIdentityEvent()
            {
                Identity = identity,
                ClientId = Id,
                OccurredOn = DateTime.UtcNow
            };
            _changes.Add(e);
            EventHandler.AllowedIdentityRemoved(this, e);
        }

        public void AddAllowedGrantType(ClientGrantType grantType)
        {
            if (_allowedGrantTypes.Contains(grantType))
            {
                throw new EntityValidationException("This grant type is already allowed.");
            }

            var e = new AddedAllowedGrantTypeEvent()
            {
                GrantType = grantType,
                ClientId = Id,
                OccurredOn = DateTime.UtcNow
            };
            _changes.Add(e);
            EventHandler.AllowedGrantTypeAdded(this, e);
        }

        public void RemoveAllowedGrantType(ClientGrantType grantType)
        {
            if (!_allowedGrantTypes.Contains(grantType))
            {
                throw new EntityValidationException("This grant type is not currently allowed.");
            }

            var e = new RemovedAllowedGrantTypeEvent()
            {
                GrantType = grantType,
                ClientId = Id,
                OccurredOn = DateTime.UtcNow
            };
            _changes.Add(e);
            EventHandler.AllowedGrantTypeRemoved(this, e);

        }

        public ClientUri AddUri(ClientUriType type, string uri)
        {
            ClientUri.ValidateUri(uri);

            if (_uris.Any(u => u.Type == type && 
                               string.Equals(u.Uri, uri, StringComparison.OrdinalIgnoreCase)))
            {
                throw new EntityValidationException("A URI with this URL and type already exists on the client!");
            }

            var e = new AddedClientUriEvent()
            {
                ClientUriId = Guid.NewGuid(),
                UriType = type,
                Uri = uri,
                ClientId = Id,
                OccurredOn = DateTime.UtcNow
            };
            _changes.Add(e);
            EventHandler.UriAdded(this, e);

            return _uris.FirstOrDefault(u => u.Id == e.ClientUriId);
        }

        public void ModifyUri(Guid clientUriId, ClientUriType type, string uri)
        {
            ClientUri.ValidateUri(uri);

            var toModify = _uris.FirstOrDefault(u => u.Id == clientUriId);
            if (null == toModify)
            {
                throw new EntityValidationException("This URI doesn't exist on this client.");
            }

            if (Active && toModify.Type == ClientUriType.Redirect && type != ClientUriType.Redirect &&
                !_uris.Any(u => u.Type == ClientUriType.Redirect && u.Id != clientUriId))
            {
                throw new EntityValidationException("Cannot change the type of this URI, an active client " +
                                                    "must have at least one Redirect URI.");
            }

            var e = new ModifiedClientUriEvent()
            {
                Uri = uri,
                UriType = type,
                ClientId = Id,
                OccurredOn = DateTime.UtcNow,
                ClientUriId = clientUriId
            };
            _changes.Add(e);
            EventHandler.UriModified(this, e);
        }

        public void RemoveUri(Guid clientUriId)
        {
            var toRemove = _uris.FirstOrDefault(u => u.Id == clientUriId);
            if (null == toRemove)
            {
                throw new EntityValidationException("This URI doesn't exist on this client.");
            }

            if (Active && !_uris.Any(uri => uri.Type == ClientUriType.Redirect && uri.Id != clientUriId))
            {
                throw new EntityValidationException("Cannot remove this URI, an active client must have at " +
                                                    "least one Redirect URI.");
            }

            var e = new RemovedClientUriEvent()
            {
                ClientUriId = clientUriId,
                ClientId = Id,
                OccurredOn = DateTime.UtcNow
            };
            _changes.Add(e);
            EventHandler.UriRemoved(this, e);
        }

        public void AddAllowedScope(string scopeName)
        {
            if (_allowedScopes.Contains(scopeName))
            {
                throw new EntityValidationException($"This scope ({scopeName}) is already allowed.");
            }

            var e = new AddedAllowedScopeEvent()
            {
                ClientId = Id,
                OccurredOn = DateTime.UtcNow,
                ScopeName = scopeName
            };
            _changes.Add(e);
            EventHandler.AllowedScopeAdded(this, e);
        }

        public void RemoveAllowedScope(string scopeName)
        {
            if (!_allowedScopes.Contains(scopeName))
            {
                throw new EntityValidationException($"This scope ({scopeName}) is not currently allowed.");
            }
            
            var e = new RemovedAllowedScopeEvent()
            {
                ClientId =  Id,
                OccurredOn = DateTime.UtcNow,
                ScopeName = scopeName
            };
            _changes.Add(e);
            EventHandler.AllowedScopeRemoved(this, e);
        }

        public void FlushChanges()
        {
            _changes.Clear();
        }
        public override bool Equals(object? obj)
        {
            var otherClient = obj as Client;

            if (null == otherClient)
            {
                return false;
            }

            return otherClient.Id.Equals(Id);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }
    }
}