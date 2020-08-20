using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
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

        public ClientService(IClientRepository clientRepository)
        {
            this._clientRepository = clientRepository;
        }
        
        public Client Get(Guid id)
        {
            return _clientRepository.Get(id);
        }

        public IEnumerable<Client> GetAll()
        {
            return _clientRepository.GetAll();
        }

        public ClientCreatedViewModel Create(CreateClientViewModel client)
        {
            ValidateClient(client);

            var secret = Guid.NewGuid().ToString();
            
            var toCreate = new Client()
            {
                Name = client.Name,
                Description =  client.Description,
                Secret = Sha256HashString(secret)
            };

            _clientRepository.Create(toCreate);

            return new ClientCreatedViewModel()
            {
                ClientId = toCreate.ClientId,
                Name = toCreate.Name,
                Description = toCreate.Description,
                Secret = secret
            };
        }

        public Client Update(UpdateClientViewModel client)
        {
            ValidateClient(client);

            var existing = _clientRepository.Get(client.ClientId);
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