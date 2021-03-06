﻿using SimpleBusinessApp.DataAccess;
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
    public class LookupDataService : IClientLookupDataService, ICompanyLookupDataService,
        IMeetingLookupDataService 
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

        public async Task<IEnumerable<LookupItem>> GetCompanyLookupAsync()
        {
            using (var ctx = _contexCreator())
            {
                return await ctx.Company.AsNoTracking()
                .Select(c =>
                new LookupItem
                {
                    Id = c.Id,
                    DisplayMember = c.Name + " " + c.OwnershipType
                })
                .ToListAsync();
            }

        }

        public async Task<List<LookupItem>> GetMeetingLookupAsync()
        {
            using (var ctx = _contexCreator())
            {
                var items = await ctx.Meetings.AsNoTracking()
                    .Select(m =>
                   new LookupItem
                   {
                       Id = m.Id,
                       DisplayMember = m.Title
                   })
                   .ToListAsync();
                return items;
            }
        }
    }
}
