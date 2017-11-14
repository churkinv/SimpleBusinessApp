using SimpleBusinessApp.DataAccess;
using SimpleBusinessApp.Model;
using SimpleBusinessApp.Data.Repositories;

namespace SimpleBusinessApp.Data.Repository
{
    public class CompanyRepository : GenericRepository<Company, ClientOrganizerDbContext>, ICompanyRepository
    {
        public CompanyRepository(ClientOrganizerDbContext context)
             : base(context)
        {
        }
    }
}
