using System.Linq;
using CloakedDagger.Common.Entities;
using Microsoft.EntityFrameworkCore;

namespace CloakedDagger.Data.Extensions
{
    public static class ClientExtensions
    {
        public static IQueryable<Client> Build(this IQueryable<Client> query)
        {
            return query.Include(c => c.ClientAllowedIdentities)
                .Include(c => c.ClientAllowedScopes)
                .ThenInclude(cas => cas.Scope)
                .Include(c => c.ClientAllowedGrantTypes)
                .Include(c => c.ClientUris);
        }
    }
}