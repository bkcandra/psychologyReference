namespace ReferencingSystem.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Foundation1 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.PaymentRecords", "PlanId", c => c.Int(nullable: false));
            AddColumn("dbo.PaymentRecords", "Status", c => c.String());
            AddColumn("dbo.PaymentRecords", "TransId", c => c.String());
            AddColumn("dbo.PaymentRecords", "PNRef", c => c.String());
            AddColumn("dbo.PaymentRecords", "Action", c => c.String());
            AddColumn("dbo.PaymentRecords", "TransFee", c => c.String());
            AddColumn("dbo.PaymentRecords", "TaxAmount", c => c.String());
            AddColumn("dbo.PaymentRecords", "Completed", c => c.Boolean(nullable: false));
            AddColumn("dbo.PaymentRecords", "Message", c => c.String());
            AlterColumn("dbo.PaymentRecords", "PayerId", c => c.String());
            AlterColumn("dbo.PaymentRecords", "Amount", c => c.String());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.PaymentRecords", "Amount", c => c.Int(nullable: false));
            AlterColumn("dbo.PaymentRecords", "PayerId", c => c.String(nullable: false));
            DropColumn("dbo.PaymentRecords", "Message");
            DropColumn("dbo.PaymentRecords", "Completed");
            DropColumn("dbo.PaymentRecords", "TaxAmount");
            DropColumn("dbo.PaymentRecords", "TransFee");
            DropColumn("dbo.PaymentRecords", "Action");
            DropColumn("dbo.PaymentRecords", "PNRef");
            DropColumn("dbo.PaymentRecords", "TransId");
            DropColumn("dbo.PaymentRecords", "Status");
            DropColumn("dbo.PaymentRecords", "PlanId");
        }
    }
}
