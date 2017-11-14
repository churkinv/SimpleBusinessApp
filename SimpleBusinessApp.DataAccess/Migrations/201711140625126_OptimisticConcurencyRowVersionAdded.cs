namespace SimpleBusinessApp.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class OptimisticConcurencyRowVersionAdded : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ClientPhoneNumber", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("dbo.Client", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
            AddColumn("dbo.Company", "RowVersion", c => c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Company", "RowVersion");
            DropColumn("dbo.Client", "RowVersion");
            DropColumn("dbo.ClientPhoneNumber", "RowVersion");
        }
    }
}
