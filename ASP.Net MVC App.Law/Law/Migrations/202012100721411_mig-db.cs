namespace Law.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class migdb : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Approveds",
                c => new
                    {
                        ApprovedId = c.Int(nullable: false),
                        ApprovedName = c.String(nullable: false, maxLength: 500),
                    })
                .PrimaryKey(t => t.ApprovedId);
            
            CreateTable(
                "dbo.Details",
                c => new
                    {
                        RuleId = c.Int(nullable: false),
                        Text = c.String(),
                        ApprovedId = c.Int(nullable: false),
                        AnnouncementNumber = c.String(maxLength: 100),
                        Article = c.String(maxLength: 10),
                    })
                .PrimaryKey(t => t.RuleId)
                .ForeignKey("dbo.Approveds", t => t.ApprovedId, cascadeDelete: true)
                .ForeignKey("dbo.Rules", t => t.RuleId)
                .Index(t => t.RuleId)
                .Index(t => t.ApprovedId);
            
            CreateTable(
                "dbo.Rules",
                c => new
                    {
                        RuleId = c.Int(nullable: false),
                        Title = c.String(),
                        ApprovalDate = c.DateTime(nullable: false),
                        AnnouncementDate = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.RuleId);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Details", "RuleId", "dbo.Rules");
            DropForeignKey("dbo.Details", "ApprovedId", "dbo.Approveds");
            DropIndex("dbo.Details", new[] { "ApprovedId" });
            DropIndex("dbo.Details", new[] { "RuleId" });
            DropTable("dbo.Rules");
            DropTable("dbo.Details");
            DropTable("dbo.Approveds");
        }
    }
}
