namespace ReferencingSystem.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Change2 : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Schools", "UniversityId", "dbo.Universities");
            DropIndex("dbo.Schools", new[] { "UniversityId" });
            CreateTable(
                "dbo.UniversityAdmins",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        UniversityId = c.Int(nullable: false),
                        ConfirmationToken = c.String(),
                        ConfirmationTokenExpiry = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.Universities", t => t.UniversityId, cascadeDelete: true)
                .ForeignKey("dbo.UserProfiles", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.UniversityId);
            
            DropTable("dbo.Schools");
        }
        
        public override void Down()
        {
            CreateTable(
                "dbo.Schools",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UniversityId = c.Int(nullable: false),
                        Name = c.String(),
                        FullAddress = c.String(),
                        Contact = c.String(),
                        CreatedUTC = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedUTC = c.Int(nullable: false),
                        ModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            DropForeignKey("dbo.UniversityAdmins", "UserId", "dbo.UserProfiles");
            DropForeignKey("dbo.UniversityAdmins", "UniversityId", "dbo.Universities");
            DropIndex("dbo.UniversityAdmins", new[] { "UniversityId" });
            DropIndex("dbo.UniversityAdmins", new[] { "UserId" });
            DropTable("dbo.UniversityAdmins");
            CreateIndex("dbo.Schools", "UniversityId");
            AddForeignKey("dbo.Schools", "UniversityId", "dbo.Universities", "Id", cascadeDelete: true);
        }
    }
}
