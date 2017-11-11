using System.Collections.Generic;
using SimpleBusinessApp.Model;
using System.Threading.Tasks;

namespace SimpleBusinessApp.Data.Repositories
{
    /// <summary>
    /// This interface shares method for uploading Client from DB by ID in async mode
    /// </summary>
    public interface IClientRepository : IGenericRepository<Client>
    {       
        void RemovePhoneNumber(ClientPhoneNumber model);
        Task<bool> HasMeetingsAsync(int clientId);
    }
}