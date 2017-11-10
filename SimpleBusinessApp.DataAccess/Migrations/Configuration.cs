namespace SimpleBusinessApp.DataAccess.Migrations
{
    using SimpleBusinessApp.Model;
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;

    internal sealed class Configuration : DbMigrationsConfiguration<SimpleBusinessApp.DataAccess.ClientOrganizerDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(SimpleBusinessApp.DataAccess.ClientOrganizerDbContext context)
        {
            context.Clients.AddOrUpdate(     // the command will create or update table in DB
                f => f.FirstName,            // I identify my Clients by First Name
                new Client { FirstName = "Thomas", LastName = "Huber" },
                new Client { FirstName = "Andreas", LastName = "Boehler" },
                new Client { FirstName = "Julia", LastName = "Huber" },
                new Client { FirstName = "Chrissi", LastName = "Egin" }
            );

            context.Company.AddOrUpdate(     
              c => c.Name,           
              new Company { Name = "Philips-Ukraine", OwnershipType = "TOV", CountryOfRegistration = "Ukraine" },
              new Company { Name = "GroupeSeb Ukraine", OwnershipType = "TOV", CountryOfRegistration = "Ukraine" },
              new Company { Name = "Samsung Electronics Co.", OwnershipType = "Ltd", CountryOfRegistration = "Republic of Korea" },
              new Company { Name = "DE Longhi Industrial", OwnershipType = " S.A.", CountryOfRegistration = "Italy" }
          );

            context.SaveChanges();

            context.ClientPhoneNumbers.AddOrUpdate(pn => pn.Number,
                new ClientPhoneNumber { Number = "+38 0670000000", ClientId = context.Clients.First().Id });


            context.Meetings.AddOrUpdate(
                m => m.Title,
                new Meeting
                {
                    Title = "Have a beer with Client",
                    DateFrom = new DateTime(2018, 1, 5),
                    DateTo = new DateTime(2018, 1, 6),
                    Clients = new List<Client>
                    {
                        context.Clients.Single(c => c.FirstName == "Batmans" && c.LastName == "Proffesor"),
                        context.Clients.Single(c => c.FirstName == "Andriy" && c.LastName == "Alch")
                    }
                });
              
            
                    
            

            //  This method will be called after migrating to the latest version.

            //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
            //  to avoid creating duplicate seed data. E.g.
            //
            //    context.People.AddOrUpdate(
            //      p => p.FullName,
            //      new Person { FullName = "Andrew Peters" },
            //      new Person { FullName = "Brice Lambson" },
            //      new Person { FullName = "Rowan Miller" }
            //    );
            //
        }
    }
}
