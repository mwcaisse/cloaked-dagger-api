using System.Linq;
using CloakedDagger.Common.Entities;
using Microsoft.EntityFrameworkCore;

namespace CloakedDagger.Data.Extensions
{
    public static class ResourceScopeExtensions
    {
        public static IQueryable<ResourceScopeEntity> Build(this IQueryable<ResourceScopeEntity> query)
        {
            return query.Include(rs => rs.ScopeEntity);
        }
    }
}