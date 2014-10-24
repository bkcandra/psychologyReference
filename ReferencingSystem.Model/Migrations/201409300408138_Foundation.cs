namespace ReferencingSystem.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Foundation : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.UserProfiles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        FirstName = c.String(nullable: false),
                        LastName = c.String(),
                        Gender = c.Int(nullable: false),
                        Contact = c.String(),
                        AltContact = c.String(),
                        UniversityID = c.Int(),
                        UniversityName = c.String(),
                        SchoolID = c.Int(),
                        SchoolName = c.String(),
                        InstitutionId = c.String(),
                        AgreeToTerms = c.Boolean(nullable: false),
                        CreatedUTC = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedUTC = c.Int(nullable: false),
                        ModifiedBy = c.String(),
                        Status = c.Int(nullable: false),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.UserSubscriptions",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        subscriptionPlanId = c.Int(nullable: false),
                        PaymentRecordId = c.Int(nullable: false),
                        ExpiryUTC = c.Int(nullable: false),
                        CreatedUTC = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedUTC = c.Int(nullable: false),
                        ModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.UserId)
                .ForeignKey("dbo.PaymentRecords", t => t.PaymentRecordId, cascadeDelete: true)
                .ForeignKey("dbo.SubscriptionPlans", t => t.subscriptionPlanId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.subscriptionPlanId)
                .Index(t => t.PaymentRecordId);
            
            CreateTable(
                "dbo.PaymentRecords",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        token = c.String(nullable: false),
                        PayerId = c.String(nullable: false),
                        shippingAddress = c.String(),
                        amount = c.Int(nullable: false),
                        CreatedUTC = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedUTC = c.Int(nullable: false),
                        ModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.SubscriptionPlans",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Price = c.Int(nullable: false),
                        RequiredRoles = c.String(),
                        LengthValue = c.Int(nullable: false),
                        LengthType = c.Int(nullable: false),
                        CreatedUTC = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedUTC = c.Int(nullable: false),
                        ModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Courses",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        CreatedUTC = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedUTC = c.Int(nullable: false),
                        ModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.EmailSettings",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        Description = c.String(),
                        EmailType = c.Int(nullable: false),
                        EmailTemplateId = c.Int(nullable: false),
                        ModifiedUTC = c.Int(nullable: false),
                        ModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.EmailTemplates",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        EmailName = c.String(nullable: false),
                        EmailSubject = c.String(nullable: false),
                        Emailcc = c.String(),
                        EmailBody = c.String(nullable: false),
                        EmailIsHTML = c.Boolean(nullable: false),
                        CreatedUTC = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedUTC = c.Int(nullable: false),
                        ModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Pages",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(nullable: false, maxLength: 250),
                        Title = c.String(nullable: false, maxLength: 250),
                        TemplateId = c.Int(nullable: false),
                        MetaTag = c.String(nullable: false, maxLength: 500),
                        MetaDescription = c.String(nullable: false, maxLength: 1000),
                        CreatedUTC = c.Int(nullable: false),
                        ModifiedUTC = c.Int(nullable: false),
                        CreatedBy = c.String(maxLength: 50),
                        ModifiedBy = c.String(maxLength: 50),
                        Content = c.String(),
                        Published = c.Boolean(nullable: false),
                        ModifiedDateTime = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.References",
                c => new
                    {
                        id = c.Int(nullable: false, identity: true),
                        RefEmail = c.String(),
                        RefUserId = c.String(maxLength: 128),
                        CourseID = c.String(),
                        Note = c.String(),
                        Message = c.String(),
                        Status = c.Int(nullable: false),
                        Process = c.Int(nullable: false),
                        Stage = c.Int(nullable: false),
                        ExpiryDate = c.String(),
                        CreatedUTC = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedUTC = c.Int(nullable: false),
                        ModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.id);
            
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
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Universities", t => t.UniversityId, cascadeDelete: true)
                .Index(t => t.UniversityId);
            
            CreateTable(
                "dbo.Universities",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(),
                        FullAddress = c.String(),
                        CreatedUTC = c.Int(nullable: false),
                        CreatedBy = c.String(),
                        ModifiedUTC = c.Int(nullable: false),
                        ModifiedBy = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Schools", "UniversityId", "dbo.Universities");
            DropForeignKey("dbo.UserSubscriptions", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.UserSubscriptions", "subscriptionPlanId", "dbo.SubscriptionPlans");
            DropForeignKey("dbo.UserSubscriptions", "PaymentRecordId", "dbo.PaymentRecords");
            DropForeignKey("dbo.UserProfiles", "UserId", "dbo.AspNetUsers");
            DropIndex("dbo.Schools", new[] { "UniversityId" });
            DropIndex("dbo.UserSubscriptions", new[] { "PaymentRecordId" });
            DropIndex("dbo.UserSubscriptions", new[] { "subscriptionPlanId" });
            DropIndex("dbo.UserSubscriptions", new[] { "UserId" });
            DropIndex("dbo.UserProfiles", new[] { "UserId" });
            DropTable("dbo.Universities");
            DropTable("dbo.Schools");
            DropTable("dbo.References");
            DropTable("dbo.Pages");
            DropTable("dbo.EmailTemplates");
            DropTable("dbo.EmailSettings");
            DropTable("dbo.Courses");
            DropTable("dbo.SubscriptionPlans");
            DropTable("dbo.PaymentRecords");
            DropTable("dbo.UserSubscriptions");
            DropTable("dbo.UserProfiles");
        }
    }
}
