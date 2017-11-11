using System.Threading.Tasks;
using SimpleBusinessApp.Model;
using SimpleBusinessApp.Data.Repositories;
using System.Collections.Generic;

namespace SimpleBusinessApp.Data.Repository
{
    public interface IMeetingRepository : IGenericRepository<Meeting>
    {
        Task<List<Client>> GetAllClientsAsync();
    }
}