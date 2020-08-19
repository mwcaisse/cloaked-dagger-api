using System.Linq;
using CloakedDagger.Common.Entities;
using Microsoft.EntityFrameworkCore;

namespace CloakedDagger.Data.Extensions
{
    public static class ResourceExtensions
    {

        public static IQueryable<Resource> Build(this IQueryable<Resource> query)
        {
            return query
                .Include(r => r.AvailableScopes)
                .ThenInclude(rs => rs.Scope);
        }
        
    }
}