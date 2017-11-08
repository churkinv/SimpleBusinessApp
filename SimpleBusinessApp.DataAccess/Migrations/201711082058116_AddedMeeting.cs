namespace SimpleBusinessApp.DataAccess.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddedMeeting : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Meeting",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Title = c.String(nullable: false, maxLength: 50),
                        DateFrom = c.DateTime(nullable: false),
                        DateTo = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MeetingClient",
                c => new
                    {
                        Meeting_Id = c.Int(nullable: false),
                        Client_Id = c.Int(nullable: false),
                    })
                .PrimaryKey(t => new { t.Meeting_Id, t.Client_Id })
                .ForeignKey("dbo.Meeting", t => t.Meeting_Id, cascadeDelete: true)
                .ForeignKey("dbo.Client", t => t.Client_Id, cascadeDelete: true)
                .Index(t => t.Meeting_Id)
                .Index(t => t.Client_Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MeetingClient", "Client_Id", "dbo.Client");
            DropForeignKey("dbo.MeetingClient", "Meeting_Id", "dbo.Meeting");
            DropIndex("dbo.MeetingClient", new[] { "Client_Id" });
            DropIndex("dbo.MeetingClient", new[] { "Meeting_Id" });
            DropTable("dbo.MeetingClient");
            DropTable("dbo.Meeting");
        }
    }
}
