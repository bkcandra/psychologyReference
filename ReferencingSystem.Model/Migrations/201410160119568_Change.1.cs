namespace ReferencingSystem.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Change1 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Courses", "CourseLevelId", "dbo.CourseLevels");
            DropIndex("dbo.Courses", new[] { "CourseLevelId" });
            CreateTable(
                "dbo.ReferenceCourses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ReferenceId = c.Int(nullable: false),
                        CourseId = c.Int(nullable: false),
                        CourseLevelId = c.Int(nullable: false),
                        CourseText = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.References", t => t.ReferenceId, cascadeDelete: true)
                .Index(t => t.ReferenceId);
            
            AddColumn("dbo.ReferenceShareRecords", "UniversityId", c => c.Int(nullable: false));
            DropColumn("dbo.References", "CourseId");
            DropColumn("dbo.ReferenceShareRecords", "SchoolID");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ReferenceShareRecords", "SchoolID", c => c.Int(nullable: false));
            AddColumn("dbo.References", "CourseId", c => c.String());
            DropForeignKey("dbo.ReferenceCourses", "ReferenceId", "dbo.References");
            DropIndex("dbo.ReferenceCourses", new[] { "ReferenceId" });
            DropColumn("dbo.ReferenceShareRecords", "UniversityId");
            DropTable("dbo.ReferenceCourses");
            CreateIndex("dbo.Courses", "CourseLevelId");
            AddForeignKey("dbo.Courses", "CourseLevelId", "dbo.CourseLevels", "Id", cascadeDelete: true);
        }
    }
}
