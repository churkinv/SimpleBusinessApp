namespace SimpleBusinessApp.DataAccess.Migrations
{
    using SimpleBusinessApp.Model;
    using System;
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
