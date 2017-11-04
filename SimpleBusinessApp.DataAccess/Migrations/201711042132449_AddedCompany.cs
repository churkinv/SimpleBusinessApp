namespace SimpleBusinessApp.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedCompany : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Company",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 50),
                        OwnershipType = c.String(nullable: false, maxLength: 50),
                        CountryOfRegistration = c.String(nullable: false, maxLength: 50),
                        HeadQuarter = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Client", "CompanyId", c => c.Int());
            CreateIndex("dbo.Client", "CompanyId");
            AddForeignKey("dbo.Client", "CompanyId", "dbo.Company", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Client", "CompanyId", "dbo.Company");
            DropIndex("dbo.Client", new[] { "CompanyId" });
            DropColumn("dbo.Client", "CompanyId");
            DropTable("dbo.Company");
        }
    }
}
