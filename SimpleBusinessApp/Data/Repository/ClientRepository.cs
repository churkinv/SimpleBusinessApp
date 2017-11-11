using SimpleBusinessApp.DataAccess;
using SimpleBusinessApp.Model;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleBusinessApp.Data.Repositories
{
    /// <summary>
    /// This class is dedicated for uploading CLients from DB
    /// </summary>
    public class ClientRepository : GenericRepository<Client, ClientOrganizerDbContext>,
        IClientRepository
    {       
        public ClientRepository(ClientOrganizerDbContext context) : 
            base(context)
        {
        }

        public override async Task<Client> GetByIdAsync(int clientId)
        {
            return await Context.Clients
                .Include(ph => ph.PhoneNumbers)
                .SingleAsync(f => f.Id == clientId); // to get data from DB

            // below is hardcoded data creation, in case no DB
            //yield return new Client { FirstName = "Thomas", LastName = "Huber" };
            //yield return new Client { FirstName = "Andreas", LastName = "Boehler" };
            //yield return new Client { FirstName = "Julia", LastName = "Huber" };
            //yield return new Client { FirstName = "Chrissi", LastName = "Egin" };
        }

        public void RemovePhoneNumber(ClientPhoneNumber model)
        {
            Context.ClientPhoneNumbers.Remove(model);
        }

        public async Task<bool> HasMeetingsAsync (int clientId)
        {
            return await Context.Meetings.AsNoTracking()
                .Include(m => m.Clients)
                .AnyAsync(m => m.Clients.Any(c => c.Id == clientId));
        }
    }
}
