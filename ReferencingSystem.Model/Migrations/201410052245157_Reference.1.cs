namespace ReferencingSystem.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Reference1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.CourseLevels",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false),
                        CreatedUTC = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedUTC = c.Int(nullable: false),
                        ModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.Courses", "Description", c => c.String());
            AddColumn("dbo.Courses", "CourseLevelId", c => c.Int());
            AddColumn("dbo.References", "UserId", c => c.String(maxLength: 128));
            AddColumn("dbo.References", "ExpiryDateUTC", c => c.Int(nullable: false));
            AddColumn("dbo.QuestionOptions", "QuestionGroupId", c => c.Int(nullable: false));
            AddColumn("dbo.RefForms", "UserType", c => c.Int(nullable: false));
            AlterColumn("dbo.Courses", "Name", c => c.String(nullable: false));
            CreateIndex("dbo.References", "UserId");
            CreateIndex("dbo.Courses", "CourseLevelId");
            AddForeignKey("dbo.References", "UserId", "dbo.AspNetUsers", "Id");
            AddForeignKey("dbo.Courses", "CourseLevelId", "dbo.CourseLevels", "Id");
            DropColumn("dbo.References", "ExpiryDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.References", "ExpiryDate", c => c.String());
            DropForeignKey("dbo.Courses", "CourseLevelId", "dbo.CourseLevels");
            DropForeignKey("dbo.References", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Courses", new[] { "CourseLevelId" });
            DropIndex("dbo.References", new[] { "UserId" });
            AlterColumn("dbo.Courses", "Name", c => c.String());
            DropColumn("dbo.RefForms", "UserType");
            DropColumn("dbo.QuestionOptions", "QuestionGroupId");
            DropColumn("dbo.References", "ExpiryDateUTC");
            DropColumn("dbo.References", "UserId");
            DropColumn("dbo.Courses", "CourseLevelId");
            DropColumn("dbo.Courses", "Description");
            DropTable("dbo.CourseLevels");
        }
    }
}
