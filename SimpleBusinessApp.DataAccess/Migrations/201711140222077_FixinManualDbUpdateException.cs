namespace SimpleBusinessApp.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixinManualDbUpdateException : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Company", "HeadQuarter", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Company", "HeadQuarter", c => c.String());
        }
    }
}
