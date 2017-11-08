using System.Threading.Tasks;
using SimpleBusinessApp.Data.Repositories;
using SimpleBusinessApp.DataAccess;
using SimpleBusinessApp.Model;
using System.Data.Entity;

namespace SimpleBusinessApp.Data.Repository
{
    public class MeetingRepository : GenericRepository<Meeting, ClientOrganizerDbContext>, IMeetingRepository
    {
        public MeetingRepository(ClientOrganizerDbContext context) : base (context)
        {

        }

        public async override Task<Meeting> GetByIdAsync(int id)
        {
            return await Context.Meetings
                .Include(m => m.Clients)
                .SingleAsync(m => m.Id == id);
        }
    }
}
