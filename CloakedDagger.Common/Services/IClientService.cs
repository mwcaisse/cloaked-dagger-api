using System;
using System.Collections;
using System.Collections.Generic;
using CloakedDagger.Common.Entities;
using CloakedDagger.Common.ViewModels;

namespace CloakedDagger.Common.Services
{
    public interface IClientService
    {
        public Client Get(Guid id);

        public IEnumerable<Client> GetAll();

        public ClientCreatedViewModel Create(CreateClientViewModel client);

        public Client Update(UpdateClientViewModel client);

        public void Delete(Guid id);

    }
}