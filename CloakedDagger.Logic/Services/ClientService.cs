using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using CloakedDagger.Common.Domain;
using CloakedDagger.Common.Entities;
using CloakedDagger.Common.Exceptions;
using CloakedDagger.Common.Repositories;
using CloakedDagger.Common.Services;
using CloakedDagger.Common.ViewModels;

namespace CloakedDagger.Logic.Services
{
    public class ClientService : IClientService
    {
        private readonly IClientRepository _clientRepository;

        private readonly IClientEventRepository _clientEventRepository;

        public ClientService(IClientRepository clientRepository, IClientEventRepository clientEventRepository)
        {
            this._clientRepository = clientRepository;
            this._clientEventRepository = clientEventRepository;
        }
        
        public ClientEntity Get(Guid id)
        {
            return _clientRepository.Get(id);
        }

        public IEnumerable<ClientEntity> GetAll()
        {
            return _clientRepository.GetAll();
        }

        public ClientCreatedViewModel Create(CreateClientViewModel vm)
        {
            ValidateClient(vm);

            var secret = Guid.NewGuid().ToString();
            
            // Create the client domain object and save it's events
            var client = new Client(vm.Name, secret, vm.Description);
            _clientEventRepository.SaveClientEvents(client.Id, client.Changes);

            // Create the view model of the client
            var toCreate = new ClientEntity()
            {
                ClientId = client.Id,
                Name = client.Name,
                Description =  client.Description,
                Secret = client.Secret
            };
            _clientRepository.Create(toCreate);

            return new ClientCreatedViewModel()
            {
                ClientId = client.Id,
                Name = client.Name,
                Description = client.Description,
                Secret = secret
            };
        }

        public ClientEntity Update(UpdateClientViewModel vm)
        {
            ValidateClient(vm);

            var clientEvents = _clientEventRepository.GetClientEvents(vm.ClientId);
            var client = Client.EventHandler.Hydrate(vm.ClientId, clientEvents);

            if (!string.Equals(client.Name, vm.Name))
            {
                client.Rename(vm.Name);
            }

            if (!string.Equals(client.Description, vm.Description))
            {
                client.Redescribe(vm.Description);
            }

            _clientEventRepository.SaveClientEvents(client.Id, client.Changes);
            
            // Update the view model
            var existing = _clientRepository.Get(vm.ClientId);
            existing.Name = client.Name;
            existing.Description = client.Description;

            _clientRepository.Update(existing);
            return _clientRepository.Get(existing.ClientId);
        }

        public void Delete(Guid id)
        {
            _clientRepository.Delete(id);
        }

        private void ValidateClient(CreateClientViewModel client)
        {
            ValidationUtils.ValidateViewModel(client);

            Guid? clientId = null;
            if (client is UpdateClientViewModel)
            {
                clientId = (client as UpdateClientViewModel).ClientId;
            }

            if (_clientRepository.ClientWithNameExists(client.Name, clientId))
            {
                throw new EntityValidationException("A client with this name already exists.");
            }
        }

        protected string Sha256HashString(string val)
        {
            if (string.IsNullOrWhiteSpace(val))
            {
                return string.Empty;
            }

            using var sha = SHA256.Create();
            
            var bytes = Encoding.UTF8.GetBytes(val);
            var hash = sha.ComputeHash(bytes);

            return Convert.ToBase64String(hash);
        }
    }
}