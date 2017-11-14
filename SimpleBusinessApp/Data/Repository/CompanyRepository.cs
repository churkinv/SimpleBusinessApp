using SimpleBusinessApp.DataAccess;
using SimpleBusinessApp.Model;
using SimpleBusinessApp.Data.Repositories;
using System.Threading.Tasks;
using System.Data.Entity;

namespace SimpleBusinessApp.Data.Repository
{
    public class CompanyRepository : GenericRepository<Company, ClientOrganizerDbContext>, ICompanyRepository
    {
        public CompanyRepository(ClientOrganizerDbContext context)
             : base(context)
        {
        }

        public async Task<bool> IsReferenceByClientAsync(int companyId)
        {
            return await Context.Clients.AsNoTracking()
                 .AnyAsync(c => c.CompanyId == companyId);
        }
    }
}
