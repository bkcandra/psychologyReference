namespace ReferencingSystem.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Change3 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.SubscriptionPlans", "IsActive", c => c.Boolean(nullable: false));
            AlterColumn("dbo.UserProfiles", "InstitutionName", c => c.String());
            DropColumn("dbo.Pages", "ModifiedDateTime");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Pages", "ModifiedDateTime", c => c.DateTime(nullable: false));
            AlterColumn("dbo.UserProfiles", "InstitutionName", c => c.String(nullable: false));
            DropColumn("dbo.SubscriptionPlans", "IsActive");
        }
    }
}
