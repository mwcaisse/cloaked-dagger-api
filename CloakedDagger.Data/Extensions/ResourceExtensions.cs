using System.Linq;
using CloakedDagger.Common.Entities;
using Microsoft.EntityFrameworkCore;

namespace CloakedDagger.Data.Extensions
{
    public static class ResourceExtensions
    {

        public static IQueryable<ResourceEntity> Build(this IQueryable<ResourceEntity> query)
        {
            return query
                .Include(r => r.AvailableScopes)
                .ThenInclude(rs => rs.ScopeEntity);
        }
        
    }
}