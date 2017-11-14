namespace SimpleBusinessApp.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class RemovedDAForRequiredFiledCompany : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Company", "OwnershipType", c => c.String(maxLength: 50));
            AlterColumn("dbo.Company", "CountryOfRegistration", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Company", "CountryOfRegistration", c => c.String(nullable: false, maxLength: 50));
            AlterColumn("dbo.Company", "OwnershipType", c => c.String(nullable: false, maxLength: 50));
        }
    }
}
