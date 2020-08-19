using System.Linq;
using CloakedDagger.Common.Entities;
using Microsoft.EntityFrameworkCore;

namespace CloakedDagger.Data.Extensions
{
    public static class ResourceScopeExtensions
    {
        public static IQueryable<ResourceScope> Build(this IQueryable<ResourceScope> query)
        {
            return query.Include(rs => rs.Scope);
        }
    }
}