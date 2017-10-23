using System.Collections.Generic;
using SimpleBusinessApp.Model;
using System.Threading.Tasks;

namespace SimpleBusinessApp.Data
{

    /// <summary>
    /// This interface shares method for uploading Client from DB by ID in async mode
    /// </summary>
    public interface IClientDataService
    {
        Task<Client> GetByIdAsync(int clientId);
        Task SaveAsync(Client client);
    }
}