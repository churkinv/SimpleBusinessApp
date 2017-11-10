using SimpleBusinessApp.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SimpleBusinessApp.Data.Lookups
{
    public interface IMeetingLookupDataService
    {
        Task<List<LookupItem>> GetMeetingLookupAsync(); 
    }
}