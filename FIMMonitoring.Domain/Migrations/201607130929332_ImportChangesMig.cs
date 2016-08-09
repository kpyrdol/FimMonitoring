namespace FIMMonitoring.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ImportChangesMig : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ImportErrors", "ErrorSendDate", c => c.DateTime());
            AddColumn("dbo.ImportErrors", "IsChecked", c => c.Boolean(nullable: false));
            AddColumn("dbo.ImportErrors", "IsBusinessValidated", c => c.Boolean(nullable: false));
            AddColumn("dbo.FimImportSources", "Disabled", c => c.Boolean(nullable: false));
            AddColumn("dbo.FimSystems", "CssStyle", c => c.String(maxLength: 200));
            AddColumn("dbo.ServiceImportErrors", "IsBusinessValidated", c => c.Boolean(nullable: false));
            DropColumn("dbo.ImportErrors", "ErrorsSendDate");
            DropColumn("dbo.ServiceImportErrors", "ErrorsSendDate");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ServiceImportErrors", "ErrorsSendDate", c => c.DateTime());
            AddColumn("dbo.ImportErrors", "ErrorsSendDate", c => c.DateTime());
            DropColumn("dbo.ServiceImportErrors", "IsBusinessValidated");
            DropColumn("dbo.FimSystems", "CssStyle");
            DropColumn("dbo.FimImportSources", "Disabled");
            DropColumn("dbo.ImportErrors", "IsBusinessValidated");
            DropColumn("dbo.ImportErrors", "IsChecked");
            DropColumn("dbo.ImportErrors", "ErrorSendDate");
        }
    }
}
