namespace ReferencingSystem.Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Reference4 : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ReferenceFiles", "FileName", c => c.String());
            AddColumn("dbo.ReferenceFiles", "Title", c => c.String());
            AddColumn("dbo.ReferenceFiles", "FileSize", c => c.Int(nullable: false));
            AddColumn("dbo.ReferenceFiles", "IsSaved", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ReferenceFiles", "IsSaved");
            DropColumn("dbo.ReferenceFiles", "FileSize");
            DropColumn("dbo.ReferenceFiles", "Title");
            DropColumn("dbo.ReferenceFiles", "FileName");
        }
    }
}
