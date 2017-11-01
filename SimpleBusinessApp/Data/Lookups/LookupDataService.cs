using SimpleBusinessApp.DataAccess;
using SimpleBusinessApp.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleBusinessApp.Data.Lookups
{
    /// <summary>
    /// This class is used in NavigationViewModel to load clients by Id
    /// </summary>
    public class LookupDataService : IClientLookupDataService
    {
        private Func<ClientOrganizerDbContext> _contexCreator;

        public LookupDataService(Func<ClientOrganizerDbContext> contextCreator)
        {
            _contexCreator = contextCreator;
        }

        public async Task<IEnumerable<LookupItem>> GetClientLookupAsync()
        {
            using (var ctx = _contexCreator())
            {
                return await ctx.Clients.AsNoTracking()
                .Select(f =>
                new LookupItem
                {
                    Id = f.Id,
                    DisplayMember = f.FirstName + " " + f.LastName
                })
                .ToListAsync();
            }

        }
    }
}
