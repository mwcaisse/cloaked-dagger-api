using System.Collections.Generic;
using CloakedDagger.Common.Entities;

namespace CloakedDagger.Common.Repositories
{
    public interface IPersistedGrantRepository
    {

        public PersistedGrantEntity Get(string id);

        public IEnumerable<PersistedGrantEntity> GetAll(string subjectId = null, string sessionId = null, 
            string clientId = null, string type = null);
        
        public void Create(PersistedGrantEntity entity);

        public void Remove(string id);

        public void RemoveAll(string subjectId = null, string sessionId = null, 
            string clientId = null, string type = null);

    }
}