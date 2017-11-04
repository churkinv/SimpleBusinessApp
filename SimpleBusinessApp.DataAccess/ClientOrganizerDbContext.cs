using SimpleBusinessApp.Model;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace SimpleBusinessApp.DataAccess
{
    public class ClientOrganizerDbContext : DbContext
    {
        public DbSet<Client> Clients { get; set; }

        public DbSet<Company> Company { get; set; }

        public ClientOrganizerDbContext() : base ("ClientOrganizerDb") // then we have to add connecition string in config file with this name
        {

        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder) // ovverading method of DbContext to have single name of tables (would be Clients now Client)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>(); // by default EF will pluralize name of the table, by this we remove that convention
            
            //modelBuilder.Configurations.Add(new ClientConfiguration()); // in case we use Fluent Api for constraints instead of Data Annotations in Client class properties

        }
    }

    //public class ClientConfiguration : EntityTypeConfiguration<Client> // fluent API
    //{

    //    public ClientConfiguration()
    //    {
    //        Property(f => f.FirstName)
    //            .IsRequired()
    //            .HasMaxLength(50);
    //    }
    //}

}

