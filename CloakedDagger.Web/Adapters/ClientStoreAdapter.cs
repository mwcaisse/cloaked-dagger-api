using System.Threading.Tasks;
using IdentityServer4.Models;
using IdentityServer4.Stores;

namespace CloakedDagger.Web.Adapters
{
    public class ClientStoreAdapter : IClientStore
    {
        public Task<Client> FindClientByIdAsync(string clientId)
        {
            throw new System.NotImplementedException();
        }
    }
}