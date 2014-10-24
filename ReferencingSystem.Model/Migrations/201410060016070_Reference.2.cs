namespace ReferencingSystem.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Reference2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Courses", "CourseLevelId", "dbo.CourseLevels");
            DropIndex("dbo.Courses", new[] { "CourseLevelId" });
            AddColumn("dbo.UserProfiles", "InstitutionName", c => c.String());
            AddColumn("dbo.UserProfiles", "Position", c => c.String());
            AddColumn("dbo.UserProfiles", "RefereeType", c => c.Int(nullable: false));
            AlterColumn("dbo.Courses", "CourseLevelId", c => c.Int(nullable: false));
            CreateIndex("dbo.Courses", "CourseLevelId");
            AddForeignKey("dbo.Courses", "CourseLevelId", "dbo.CourseLevels", "Id", cascadeDelete: true);
            DropColumn("dbo.References", "Message");
            DropColumn("dbo.UserProfiles", "UniversityName");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserProfiles", "UniversityName", c => c.String());
            AddColumn("dbo.References", "Message", c => c.String());
            DropForeignKey("dbo.Courses", "CourseLevelId", "dbo.CourseLevels");
            DropIndex("dbo.Courses", new[] { "CourseLevelId" });
            AlterColumn("dbo.Courses", "CourseLevelId", c => c.Int());
            DropColumn("dbo.UserProfiles", "RefereeType");
            DropColumn("dbo.UserProfiles", "Position");
            DropColumn("dbo.UserProfiles", "InstitutionName");
            CreateIndex("dbo.Courses", "CourseLevelId");
            AddForeignKey("dbo.Courses", "CourseLevelId", "dbo.CourseLevels", "Id");
        }
    }
}
