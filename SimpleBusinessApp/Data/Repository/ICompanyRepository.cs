using SimpleBusinessApp.Data.Repositories;
using SimpleBusinessApp.Model;
using System.Threading.Tasks;

namespace SimpleBusinessApp.Data.Repository
{
    public interface ICompanyRepository : IGenericRepository<Company>
    {
        Task<bool> IsReferenceByClientAsync(int companyId);
    }
}
