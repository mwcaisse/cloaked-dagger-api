using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CloakedDagger.Common.Entities;
using CloakedDagger.Common.Repositories;
using IdentityServer4.Models;
using IdentityServer4.Stores;

namespace CloakedDagger.Web.Adapters
{
    public class PersistedGrantStoreAdapter : IPersistedGrantStore
    {

        private readonly IPersistedGrantRepository _repository;

        public PersistedGrantStoreAdapter(IPersistedGrantRepository repository)
        {
            this._repository = repository;
        }
        
        public Task StoreAsync(PersistedGrant grant)
        {
            _repository.Create(ToEntity(grant));
            return Task.CompletedTask;
        }

        public Task<PersistedGrant> GetAsync(string key)
        {
            var grant = _repository.Get(key);
            return Task.FromResult(FromEntity(grant));
        }

        public Task<IEnumerable<PersistedGrant>> GetAllAsync(PersistedGrantFilter filter)
        {
            var grants = _repository
                .GetAll(filter.SubjectId, filter.SessionId, filter.ClientId, filter.Type)
                .Select(FromEntity)
                .ToList();
            return Task.FromResult((IEnumerable<PersistedGrant>) grants);
        }

        public Task RemoveAsync(string key)
        {
            _repository.Remove(key);
            return Task.CompletedTask;
        }

        public Task RemoveAllAsync(PersistedGrantFilter filter)
        {
            _repository.RemoveAll(filter.SubjectId, filter.SessionId, filter.ClientId, filter.Type);
            return Task.CompletedTask;
        }

        private static PersistedGrantEntity ToEntity(PersistedGrant grant)
        {
            return new()
            {
                Id = grant.Key,
                Type = grant.Type,
                SubjectId = grant.SubjectId,
                SessionId = grant.SubjectId,
                ClientId = grant.ClientId,
                Description = grant.Description,
                CreateDate = grant.CreationTime,
                ExpirationDate = grant.Expiration,
                ConsumedDate = grant.ConsumedTime,
                Data = grant.Data
            };
        }

        private static PersistedGrant FromEntity(PersistedGrantEntity entity)
        {
            return new ()
            {
                Key = entity.Id,
                Type = entity.Type,
                SubjectId = entity.SubjectId,
                SessionId = entity.SessionId,
                ClientId = entity.ClientId,
                Description = entity.Description,
                CreationTime = entity.CreateDate,
                Expiration = entity.ExpirationDate,
                ConsumedTime = entity.ConsumedDate,
                Data = entity.Data
            };
        }
    }
}