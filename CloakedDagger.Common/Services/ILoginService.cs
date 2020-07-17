using System.Security.Claims;

namespace CloakedDagger.Common.Services
{
    public interface ILoginService
    {
        public ClaimsPrincipal Login(string username, string password);
    }
}