namespace ReferencingSystem.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Reference : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.DownloadRecords",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        ReferenceShareRecordId = c.Int(nullable: false),
                        IpAddress = c.String(),
                        UserId = c.String(nullable: false),
                        CreatedUTC = c.Int(nullable: false),
                        CreatedBy = c.String(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.ReferenceShareRecords", t => t.ReferenceShareRecordId, cascadeDelete: true)
                .Index(t => t.ReferenceShareRecordId);
            
            CreateTable(
                "dbo.ReferenceShareRecords",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        ReferenceId = c.Int(nullable: false),
                        LinkToken = c.String(nullable: false),
                        SchoolID = c.Int(nullable: false),
                        ClickCount = c.Int(nullable: false),
                        CreatedUTC = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedUTC = c.Int(nullable: false),
                        ModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.id)
                .ForeignKey("dbo.References", t => t.ReferenceId, cascadeDelete: true)
                .Index(t => t.ReferenceId);
            
            CreateTable(
                "dbo.ReferenceAnswers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ReferenceId = c.Int(nullable: false),
                        QuestionId = c.Int(nullable: false),
                        Answer = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.References", t => t.ReferenceId, cascadeDelete: true)
                .Index(t => t.ReferenceId);
            
            CreateTable(
                "dbo.ReferenceFiles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ReferenceId = c.Int(nullable: false),
                        FileByte = c.Binary(),
                        ModifiedUTC = c.Int(nullable: false),
                        ModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.References", t => t.ReferenceId, cascadeDelete: true)
                .Index(t => t.ReferenceId);
            
            CreateTable(
                "dbo.Questions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FormId = c.Int(nullable: false),
                        Type = c.Int(nullable: false),
                        Order = c.Int(nullable: false),
                        Group = c.Int(nullable: false),
                        Text = c.String(),
                        CreatedUTC = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedUTC = c.Int(nullable: false),
                        ModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.RefForms", t => t.FormId, cascadeDelete: true)
                .Index(t => t.FormId);
            
            CreateTable(
                "dbo.QuestionOptions",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        QuestionId = c.Int(nullable: false),
                        Text = c.String(),
                        Order = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Questions", t => t.QuestionId, cascadeDelete: true)
                .Index(t => t.QuestionId);
            
            CreateTable(
                "dbo.RefForms",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Type = c.Int(nullable: false),
                        Name = c.String(),
                        Readable = c.String(),
                        Status = c.Int(nullable: false),
                        CreatedUTC = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedUTC = c.Int(nullable: false),
                        ModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.References", "TransId", c => c.String(nullable: false));
            AddColumn("dbo.References", "Token", c => c.String(nullable: false));
            AddColumn("dbo.References", "IsActive", c => c.Boolean(nullable: false));
            AlterColumn("dbo.References", "RefEmail", c => c.String(nullable: false));
            DropColumn("dbo.References", "Process");
            DropColumn("dbo.References", "Stage");
        }
        
        public override void Down()
        {
            AddColumn("dbo.References", "Stage", c => c.Int(nullable: false));
            AddColumn("dbo.References", "Process", c => c.Int(nullable: false));
            DropForeignKey("dbo.Questions", "FormId", "dbo.RefForms");
            DropForeignKey("dbo.QuestionOptions", "QuestionId", "dbo.Questions");
            DropForeignKey("dbo.ReferenceShareRecords", "ReferenceId", "dbo.References");
            DropForeignKey("dbo.ReferenceFiles", "ReferenceId", "dbo.References");
            DropForeignKey("dbo.ReferenceAnswers", "ReferenceId", "dbo.References");
            DropForeignKey("dbo.DownloadRecords", "ReferenceShareRecordId", "dbo.ReferenceShareRecords");
            DropIndex("dbo.QuestionOptions", new[] { "QuestionId" });
            DropIndex("dbo.Questions", new[] { "FormId" });
            DropIndex("dbo.ReferenceFiles", new[] { "ReferenceId" });
            DropIndex("dbo.ReferenceAnswers", new[] { "ReferenceId" });
            DropIndex("dbo.ReferenceShareRecords", new[] { "ReferenceId" });
            DropIndex("dbo.DownloadRecords", new[] { "ReferenceShareRecordId" });
            AlterColumn("dbo.References", "RefEmail", c => c.String());
            DropColumn("dbo.References", "IsActive");
            DropColumn("dbo.References", "Token");
            DropColumn("dbo.References", "TransId");
            DropTable("dbo.RefForms");
            DropTable("dbo.QuestionOptions");
            DropTable("dbo.Questions");
            DropTable("dbo.ReferenceFiles");
            DropTable("dbo.ReferenceAnswers");
            DropTable("dbo.ReferenceShareRecords");
            DropTable("dbo.DownloadRecords");
        }
    }
}
