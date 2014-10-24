namespace ReferencingSystem.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Reference3 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Navigations",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Value = c.String(nullable: false),
                        Text = c.String(nullable: false),
                        ValueType = c.String(nullable: false),
                        Order = c.Int(nullable: false),
                        CreatedUTC = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedUTC = c.Int(nullable: false),
                        ModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.WebAssets",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FileName = c.String(nullable: false),
                        Title = c.String(),
                        Description = c.String(),
                        Extension = c.String(),
                        Size = c.Int(nullable: false),
                        File = c.Binary(nullable: false),
                        CreatedUTC = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedUTC = c.Int(nullable: false),
                        ModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.WebConfigs",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CreatedUTC = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedUTC = c.Int(nullable: false),
                        ModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.References", "FormId", c => c.Int());
            AddColumn("dbo.Questions", "IsActive", c => c.Boolean(nullable: false));
            AddColumn("dbo.RefForms", "IsActive", c => c.Boolean(nullable: false));
            AlterColumn("dbo.UserProfiles", "InstitutionName", c => c.String(nullable: false));
            CreateIndex("dbo.References", "FormId");
            AddForeignKey("dbo.References", "FormId", "dbo.RefForms", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.References", "FormId", "dbo.RefForms");
            DropIndex("dbo.References", new[] { "FormId" });
            AlterColumn("dbo.UserProfiles", "InstitutionName", c => c.String());
            DropColumn("dbo.RefForms", "IsActive");
            DropColumn("dbo.Questions", "IsActive");
            DropColumn("dbo.References", "FormId");
            DropTable("dbo.WebConfigs");
            DropTable("dbo.WebAssets");
            DropTable("dbo.Navigations");
        }
    }
}
