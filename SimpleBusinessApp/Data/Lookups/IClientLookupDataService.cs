using System.Collections.Generic;
using System.Threading.Tasks;
using SimpleBusinessApp.Model;

namespace SimpleBusinessApp.Data.Lookups
{
    public interface IClientLookupDataService
    {
        Task<IEnumerable<LookupItem>> GetClientLookupAsync();
    }
}