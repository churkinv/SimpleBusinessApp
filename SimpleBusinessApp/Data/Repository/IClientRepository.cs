using System.Collections.Generic;
using SimpleBusinessApp.Model;
using System.Threading.Tasks;

namespace SimpleBusinessApp.Data.Repositories
{

    /// <summary>
    /// This interface shares method for uploading Client from DB by ID in async mode
    /// </summary>
    public interface IClientRepository
    {
        Task<Client> GetByIdAsync(int clientId);
        Task SaveAsync();
        bool HasChanges();
        void Add(Client client);
        void Remove(Client model);
        void RemovePhoneNumber(ClientPhoneNumber model);
    }
}