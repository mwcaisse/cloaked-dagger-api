using System;
using System.Collections;
using System.Collections.Generic;
using CloakedDagger.Common.Entities;
using CloakedDagger.Common.ViewModels;

namespace CloakedDagger.Common.Services
{
    public interface IClientService
    {
        public ClientEntity Get(Guid id);

        public IEnumerable<ClientEntity> GetAll();

        public ClientCreatedViewModel Create(CreateClientViewModel client);

        public ClientEntity Update(UpdateClientViewModel client);

        public void Delete(Guid id);

    }
}