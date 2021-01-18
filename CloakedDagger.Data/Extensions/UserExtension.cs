using System.Linq;
using CloakedDagger.Common.Entities;
using Microsoft.EntityFrameworkCore;

namespace CloakedDagger.Data.Extensions
{
    public static class UserExtension
    {
        public static IQueryable<UserEntity> Build(this IQueryable<UserEntity> query)
        {
            return query
                .Include(u => u.Roles)
                .ThenInclude(ur => ur.Role);
        }
    }
}