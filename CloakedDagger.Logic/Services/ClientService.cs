using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using CloakedDagger.Common.Domain;
using CloakedDagger.Common.Domain.Events.Client;
using CloakedDagger.Common.Entities;
using CloakedDagger.Common.Enums;
using CloakedDagger.Common.Exceptions;
using CloakedDagger.Common.Mapper;
using CloakedDagger.Common.Repositories;
using CloakedDagger.Common.Services;
using CloakedDagger.Common.ViewModels;
using DasCookbook.Common.Exceptions;

namespace CloakedDagger.Logic.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;

        private readonly IClientEventRepository _clientEventRepository;

        private readonly IResourceScopeRepository _resourceScopeRepository;

        public ClientService(IClientRepository clientRepository, IClientEventRepository clientEventRepository,
            IResourceScopeRepository resourceScopeRepository)
        {
            this._clientRepository = clientRepository;
            this._clientEventRepository = clientEventRepository;
            this._resourceScopeRepository = resourceScopeRepository;
        }
        
        public Client Get(Guid id)
        {
            return HydrateClient(id);
        }

        public IEnumerable<ClientViewModel> GetAll()
        {
            return _clientEventRepository.GetAllClientEvents()
                .Select(p =>Client.EventHandler.Hydrate(p.Key, p.Value))
                .ToViewModel();
        }

        public ClientCreatedViewModel Create(UpdateClientViewModel vm)
        {
            ValidateClient(vm);

            var secret = Guid.NewGuid().ToString();
            
            // Create the client domain object and save it's events
            var client = new Client(vm.Name, secret, vm.Description);
            
            //Save the client
            SaveClient(client);
            
            return new ClientCreatedViewModel()
            {
                Id = client.Id,
                Name = client.Name,
                Description = client.Description,
                Secret = secret,
                Active = client.Active
            };
        }

        public ClientViewModel Update(Guid id, UpdateClientViewModel vm)
        {
            ValidateClient(vm, id);

            var client = PerformActionOnClient(id, c =>
            {
                if (!string.Equals(c.Name, vm.Name))
                {
                    c.Rename(vm.Name);
                }

                if (!string.Equals(c.Description, vm.Description))
                {
                    c.Redescribe(vm.Description);
                }
            });
            return client.ToViewModel();
        }

        public void Activate(Guid id)
        {
            PerformActionOnClient(id, c => c.Activate());
        }

        public void Deactivate(Guid id)
        {
            PerformActionOnClient(id, c => c.Deactivate());
        }

        public void AddAllowedIdentity(Guid id, Identity identity)
        {
            PerformActionOnClient(id, c => c.AddAllowedIdentity(identity));
        }

        public void RemoveAllowedIdentity(Guid id, Identity identity)
        {
            PerformActionOnClient(id, c => c.RemoveAllowedIdentity(identity));
        }

        public void AddAllowedGrantType(Guid id, ClientGrantType grantType)
        {
            PerformActionOnClient(id, c => c.AddAllowedGrantType(grantType));
        }

        public void RemoveAllowedGrantType(Guid id, ClientGrantType grantType)
        {
            PerformActionOnClient(id, c => c.RemoveAllowedGrantType(grantType));
        }

        public ClientUriViewModel AddUri(Guid id, UpdateClientUriViewModel vm)
        {
            var addedUri = PerformActionOnClientForResult(id, c => c.AddUri(vm.Type, vm.Uri));
            return new ClientUriViewModel()
            {
                Id = addedUri.Id,
                Type = addedUri.Type,
                Uri = addedUri.Uri
            };
        }

        public void UpdateUri(Guid id, Guid clientUriId, UpdateClientUriViewModel vm)
        {
            PerformActionOnClient(id, c => c.ModifyUri(clientUriId, vm.Type, vm.Uri));
        }

        public void RemoveUri(Guid id, Guid uriId)
        {
            PerformActionOnClient(id, c => c.RemoveUri(uriId));
        }

        public void AddAllowedScope(Guid id, string scopeName)
        {
            if (!_resourceScopeRepository.ExistsOnAnyResource(scopeName))
            {
                throw new EntityValidationException("This scope doesn't exist on any resource!");
            }
            PerformActionOnClient(id, c => c.AddAllowedScope(scopeName));
        }

        public void RemoveAllowedScope(Guid id, string scopeName)
        {
            PerformActionOnClient(id, c => c.RemoveAllowedScope(scopeName));
        }

        private Client PerformActionOnClient(Guid id, Action<Client> action)
        {
            var client = HydrateClient(id);
            if (null == client)
            {
                throw new EntityNotFoundException($"No client with id {id} exists!");
            }

            action(client);
            
            SaveClient(client);
            return client;
        }

        private T PerformActionOnClientForResult<T>(Guid id, Func<Client, T> action)
        {
            var client = HydrateClient(id);
            if (null == client)
            {
                throw new EntityNotFoundException($"No client with id {id} exists!");
            }

            var res = action(client);
            
            SaveClient(client);
            return res;
        }

        private Client HydrateClient(Guid id)
        {
            var clientEvents = _clientEventRepository.GetClientEvents(id).ToList();
            if (!clientEvents.Any())
            {
                return null;
            }
            return Client.EventHandler.Hydrate(id, clientEvents);
        }
        
        private void SaveClient(Client client)
        {
            if (client.Changes.Any())
            {
                _clientEventRepository.SaveClientEvents(client.Id, client.Changes);

                if (client.Changes.Any(e => e is ClientCreatedEvent))
                {
                    var toCreate = new ClientEntity()
                    {
                        ClientId = client.Id,
                        Name = client.Name,
                        Description =  client.Description,
                        Secret = client.Secret
                    };
                    _clientRepository.Create(toCreate);
                }
                else if (client.Changes.Any(e => e is ClientRenamedEvent || e is ClientRedescribedEvent))
                {
                    // We are creating it above with the current Name/Description of the client, so even if they
                    // were changed during processing, it will be created with the current Name/Description
                    // Also because of how this service works, it won't have both
                    
                    var clientEntity = _clientRepository.Get(client.Id);
                    clientEntity.Name = client.Name;
                    clientEntity.Description = client.Description;
                    _clientRepository.Update(clientEntity);
                }
                
                client.FlushChanges();
            }
        }
        
        public void Delete(Guid id)
        {
            //TODO: Revisit this.
            _clientRepository.Delete(id);
        }

        private void ValidateClient(UpdateClientViewModel client, Guid? clientId = null)
        {
            ValidationUtils.ValidateViewModel(client);

            if (_clientRepository.ClientWithNameExists(client.Name, clientId))
            {
                throw new EntityValidationException("A client with this name already exists.");
            }
        }
    }
}