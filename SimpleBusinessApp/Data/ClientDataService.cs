using SimpleBusinessApp.DataAccess;
using SimpleBusinessApp.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleBusinessApp.Data
{

    /// <summary>
    /// This class is dedicated for uploading CLients from DB
    /// </summary>
    public class ClientDataService : IClientDataService
    {
        private Func<ClientOrganizerDbContext> _contextCreator;

        public ClientDataService(Func<ClientOrganizerDbContext> contextCreator) // ?we use Func to "get" autofac injection?
        {
            _contextCreator = contextCreator;
        }

        public async Task <Client> GetByIdAsync(int clientId)
        {
            using (var ctx = _contextCreator())
            {
                return await ctx.Clients.AsNoTracking().SingleAsync(f=> f.Id == clientId); // to get data from DB
            }

            // below is hardcoded data creation, in case no DB
            //yield return new Client { FirstName = "Thomas", LastName = "Huber" };
            //yield return new Client { FirstName = "Andreas", LastName = "Boehler" };
            //yield return new Client { FirstName = "Julia", LastName = "Huber" };
            //yield return new Client { FirstName = "Chrissi", LastName = "Egin" };
        }

        public async Task SaveAsync(Client client)
        {
            using (var ctx = _contextCreator())
            {
                ctx.Clients.Attach(client);
                ctx.Entry(client).State = EntityState.Modified;
                await ctx.SaveChangesAsync();
            }
        }
    }
}
