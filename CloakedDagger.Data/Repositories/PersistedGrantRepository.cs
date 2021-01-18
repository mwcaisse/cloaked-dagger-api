using System.Collections.Generic;
using System.Linq;
using CloakedDagger.Common.Entities;
using CloakedDagger.Common.Repositories;

namespace CloakedDagger.Data.Repositories
{
    public class PersistedGrantRepository : IPersistedGrantRepository
    {

        private readonly CloakedDaggerDbContext _db;

        public PersistedGrantRepository(CloakedDaggerDbContext db)
        {
            this._db = db;
        }
        
        public PersistedGrantEntity Get(string id)
        {
            return _db.PersistedGrants.FirstOrDefault(pg => pg.Id == id);
        }

        public IEnumerable<PersistedGrantEntity> GetAll(string subjectId = null, string sessionId = null, string clientId = null, string type = null)
        {
            var query = _db.PersistedGrants.AsQueryable();

            if (!string.IsNullOrWhiteSpace(subjectId))
            {
                query = query.Where(pg => pg.SubjectId == subjectId);
            }
            if (!string.IsNullOrWhiteSpace(sessionId))
            {
                query = query.Where(pg => pg.SessionId == sessionId);
            }
            if (!string.IsNullOrWhiteSpace(clientId))
            {
                query = query.Where(pg => pg.ClientId == clientId);
            }
            if (!string.IsNullOrWhiteSpace(type))
            {
                query = query.Where(pg => pg.Type == type);
            }

            return query;
        }

        public void Create(PersistedGrantEntity entity)
        {
            _db.PersistedGrants.Add(entity);
            _db.SaveChanges();
        }

        public void Remove(string id)
        {
            var toRemove = _db.PersistedGrants.FirstOrDefault(pg => pg.Id == id);
            if (null != toRemove)
            {
                _db.PersistedGrants.Remove(toRemove);
                _db.SaveChanges();
            }
        }

        public void RemoveAll(string subjectId = null, string sessionId = null, string clientId = null, string type = null)
        {
            _db.PersistedGrants.RemoveRange(GetAll(subjectId, sessionId, clientId, type));
            _db.SaveChanges();
        }
    }
}