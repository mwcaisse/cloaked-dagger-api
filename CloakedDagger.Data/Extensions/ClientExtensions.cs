using System.Linq;
using CloakedDagger.Common.Entities;
using Microsoft.EntityFrameworkCore;

namespace CloakedDagger.Data.Extensions
{
    public static class ClientExtensions
    {
        public static IQueryable<ClientEntity> Build(this IQueryable<ClientEntity> query)
        {
            return query.Include(c => c.ClientAllowedIdentities)
                .Include(c => c.ClientAllowedScopes)
                .ThenInclude(cas => cas.ScopeEntity)
                .Include(c => c.ClientAllowedGrantTypes)
                .Include(c => c.ClientUris);
        }
    }
}