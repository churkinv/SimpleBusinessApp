using SimpleBusinessApp.DataAccess;
using SimpleBusinessApp.Model;
using System.Data.Entity;
using System.Threading.Tasks;

namespace SimpleBusinessApp.Data.Repositories
{
    /// <summary>
    /// This class is dedicated for uploading CLients from DB
    /// </summary>
    public class ClientRepository : IClientRepository
    {
        private ClientOrganizerDbContext _context;

        public ClientRepository(ClientOrganizerDbContext context) // Func<ClientOrganizerDbContext> contextCreator ?we used Func to "get" autofac injection? --> then changed it to use context
        {
            _context = context;
        }

        public async Task<Client> GetByIdAsync(int clientId)
        {
            return await _context.Clients.SingleAsync(f => f.Id == clientId); // to get data from DB
            
            // below is hardcoded data creation, in case no DB
            //yield return new Client { FirstName = "Thomas", LastName = "Huber" };
            //yield return new Client { FirstName = "Andreas", LastName = "Boehler" };
            //yield return new Client { FirstName = "Julia", LastName = "Huber" };
            //yield return new Client { FirstName = "Chrissi", LastName = "Egin" };
        }

        public async Task SaveAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
