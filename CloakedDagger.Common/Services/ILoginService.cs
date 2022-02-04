using System;
using System.Security.Claims;
using CloakedDagger.Common.ViewModels;

namespace CloakedDagger.Common.Services
{
    public interface ILoginService
    {
        public ClaimsPrincipal Login(string username, string password);

        public ClaimsPrincipal RefreshClaimsPrincipalForUser(Guid userId);

        public UserViewModel GetUserViewModelFromPrincipal(ClaimsPrincipal user);
    }
}