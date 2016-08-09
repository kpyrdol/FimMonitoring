namespace FIMMonitoring.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ImportErrorMig : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ImportErrors", "Description", c => c.String());
            AddColumn("dbo.ImportErrors", "SystemId", c => c.Int(nullable: false));
            AddColumn("dbo.ImportErrors", "Sent", c => c.Boolean(nullable: false));
            AddColumn("dbo.ImportErrors", "FileCheckId", c => c.Int());
            AlterColumn("dbo.FileChecks", "FileName", c => c.String(maxLength: 200));
            AlterColumn("dbo.FileChecks", "Parsed", c => c.Boolean(nullable: false));
            AlterColumn("dbo.ImportErrors", "ImportId", c => c.Int());
            AlterColumn("dbo.ImportErrors", "SourceId", c => c.Int());
            AlterColumn("dbo.ImportErrors", "CustomerId", c => c.Int());
            AlterColumn("dbo.ImportErrors", "CarrierId", c => c.Int());
            CreateIndex("dbo.ImportErrors", "FileCheckId");
            AddForeignKey("dbo.ImportErrors", "FileCheckId", "dbo.FileChecks", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.ImportErrors", "FileCheckId", "dbo.FileChecks");
            DropIndex("dbo.ImportErrors", new[] { "FileCheckId" });
            AlterColumn("dbo.ImportErrors", "CarrierId", c => c.Int(nullable: false));
            AlterColumn("dbo.ImportErrors", "CustomerId", c => c.Int(nullable: false));
            AlterColumn("dbo.ImportErrors", "SourceId", c => c.Int(nullable: false));
            AlterColumn("dbo.ImportErrors", "ImportId", c => c.Int(nullable: false));
            AlterColumn("dbo.FileChecks", "Parsed", c => c.String(maxLength: 100));
            AlterColumn("dbo.FileChecks", "FileName", c => c.String(maxLength: 100));
            DropColumn("dbo.ImportErrors", "FileCheckId");
            DropColumn("dbo.ImportErrors", "Sent");
            DropColumn("dbo.ImportErrors", "SystemId");
            DropColumn("dbo.ImportErrors", "Description");
        }
    }
}
