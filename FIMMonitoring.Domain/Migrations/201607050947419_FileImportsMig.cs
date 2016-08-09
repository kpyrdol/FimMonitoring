namespace FIMMonitoring.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FileImportsMig : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FimCarriers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 200),
                        ForeignId = c.Int(nullable: false),
                        SystemId = c.Int(nullable: false),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FimSystems", t => t.SystemId)
                .Index(t => t.SystemId);
            
            CreateTable(
                "dbo.FimCustomerCarriers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FimCustomerId = c.Int(nullable: false),
                        FimCarrierId = c.Int(nullable: false),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FimCarriers", t => t.FimCarrierId)
                .ForeignKey("dbo.FimCustomers", t => t.FimCustomerId)
                .Index(t => t.FimCustomerId)
                .Index(t => t.FimCarrierId);
            
            CreateTable(
                "dbo.FimCustomers",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 200),
                        ForeignId = c.Int(nullable: false),
                        SystemId = c.Int(nullable: false),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FimSystems", t => t.SystemId)
                .Index(t => t.SystemId);
            
            CreateTable(
                "dbo.FimImportSources",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        CarrierId = c.Int(nullable: false),
                        Name = c.String(maxLength: 200),
                        ForeignId = c.Int(nullable: false),
                        SystemId = c.Int(nullable: false),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FimCarriers", t => t.CarrierId)
                .ForeignKey("dbo.FimSystems", t => t.SystemId)
                .Index(t => t.CarrierId)
                .Index(t => t.SystemId);
            
            CreateTable(
                "dbo.FimSystems",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Name = c.String(maxLength: 200),
                        Url = c.String(maxLength: 200),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.ServiceImportErrors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ErrorSource = c.Int(nullable: false),
                        ErrorLevel = c.Int(nullable: false),
                        ErrorType = c.Int(),
                        ErrorDate = c.DateTime(),
                        ErrorsSendDate = c.DateTime(),
                        CreatedAt = c.DateTime(nullable: false),
                        IsValidated = c.Boolean(nullable: false),
                        IsParsed = c.Boolean(nullable: false),
                        IsDownloaded = c.Boolean(nullable: false),
                        IsSent = c.Boolean(nullable: false),
                        Description = c.String(),
                        CustomerName = c.String(maxLength: 200),
                        CarrierName = c.String(maxLength: 200),
                        SourceName = c.String(maxLength: 200),
                        Guid = c.Guid(nullable: false),
                        ImportId = c.Int(),
                        SourceId = c.Int(),
                        CustomerId = c.Int(),
                        CarrierId = c.Int(),
                        FileCheckId = c.Int(),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.FileChecks", t => t.FileCheckId)
                .Index(t => t.FileCheckId);
            
            AddColumn("dbo.ImportErrors", "Description", c => c.String());
            AddColumn("dbo.ImportErrors", "SystemId", c => c.Int(nullable: false));
            AddColumn("dbo.ImportErrors", "Filename", c => c.String(maxLength: 200));
            AddColumn("dbo.ImportErrors", "FimImportSourceId", c => c.Int());
            AddColumn("dbo.ImportErrors", "FimCustomerId", c => c.Int());
            AddColumn("dbo.ImportErrors", "FimCarrierId", c => c.Int());
            AlterColumn("dbo.FileChecks", "FileName", c => c.String(maxLength: 200));
            AlterColumn("dbo.FileChecks", "Parsed", c => c.Boolean(nullable: false));
            AlterColumn("dbo.ImportErrors", "ImportId", c => c.Int());
            CreateIndex("dbo.ImportErrors", "SystemId");
            CreateIndex("dbo.ImportErrors", "FimImportSourceId");
            CreateIndex("dbo.ImportErrors", "FimCustomerId");
            CreateIndex("dbo.ImportErrors", "FimCarrierId");
            AddForeignKey("dbo.ImportErrors", "FimCarrierId", "dbo.FimCarriers", "Id");
            AddForeignKey("dbo.ImportErrors", "FimCustomerId", "dbo.FimCustomers", "Id");
            AddForeignKey("dbo.ImportErrors", "FimImportSourceId", "dbo.FimImportSources", "Id");
            AddForeignKey("dbo.ImportErrors", "SystemId", "dbo.FimSystems", "Id");
            DropColumn("dbo.ImportErrors", "SourceName");
            DropColumn("dbo.ImportErrors", "SourceId");
            DropColumn("dbo.ImportErrors", "CustomerId");
            DropColumn("dbo.ImportErrors", "CarrierId");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ImportErrors", "CarrierId", c => c.Int(nullable: false));
            AddColumn("dbo.ImportErrors", "CustomerId", c => c.Int(nullable: false));
            AddColumn("dbo.ImportErrors", "SourceId", c => c.Int(nullable: false));
            AddColumn("dbo.ImportErrors", "SourceName", c => c.String(maxLength: 200));
            DropForeignKey("dbo.ServiceImportErrors", "FileCheckId", "dbo.FileChecks");
            DropForeignKey("dbo.FimCarriers", "SystemId", "dbo.FimSystems");
            DropForeignKey("dbo.FimCustomers", "SystemId", "dbo.FimSystems");
            DropForeignKey("dbo.ImportErrors", "SystemId", "dbo.FimSystems");
            DropForeignKey("dbo.FimImportSources", "SystemId", "dbo.FimSystems");
            DropForeignKey("dbo.ImportErrors", "FimImportSourceId", "dbo.FimImportSources");
            DropForeignKey("dbo.FimImportSources", "CarrierId", "dbo.FimCarriers");
            DropForeignKey("dbo.ImportErrors", "FimCustomerId", "dbo.FimCustomers");
            DropForeignKey("dbo.ImportErrors", "FimCarrierId", "dbo.FimCarriers");
            DropForeignKey("dbo.FimCustomerCarriers", "FimCustomerId", "dbo.FimCustomers");
            DropForeignKey("dbo.FimCustomerCarriers", "FimCarrierId", "dbo.FimCarriers");
            DropIndex("dbo.ServiceImportErrors", new[] { "FileCheckId" });
            DropIndex("dbo.FimImportSources", new[] { "SystemId" });
            DropIndex("dbo.FimImportSources", new[] { "CarrierId" });
            DropIndex("dbo.ImportErrors", new[] { "FimCarrierId" });
            DropIndex("dbo.ImportErrors", new[] { "FimCustomerId" });
            DropIndex("dbo.ImportErrors", new[] { "FimImportSourceId" });
            DropIndex("dbo.ImportErrors", new[] { "SystemId" });
            DropIndex("dbo.FimCustomers", new[] { "SystemId" });
            DropIndex("dbo.FimCustomerCarriers", new[] { "FimCarrierId" });
            DropIndex("dbo.FimCustomerCarriers", new[] { "FimCustomerId" });
            DropIndex("dbo.FimCarriers", new[] { "SystemId" });
            AlterColumn("dbo.ImportErrors", "ImportId", c => c.Int(nullable: false));
            AlterColumn("dbo.FileChecks", "Parsed", c => c.String(maxLength: 100));
            AlterColumn("dbo.FileChecks", "FileName", c => c.String(maxLength: 100));
            DropColumn("dbo.ImportErrors", "FimCarrierId");
            DropColumn("dbo.ImportErrors", "FimCustomerId");
            DropColumn("dbo.ImportErrors", "FimImportSourceId");
            DropColumn("dbo.ImportErrors", "Filename");
            DropColumn("dbo.ImportErrors", "SystemId");
            DropColumn("dbo.ImportErrors", "Description");
            DropTable("dbo.ServiceImportErrors");
            DropTable("dbo.FimSystems");
            DropTable("dbo.FimImportSources");
            DropTable("dbo.FimCustomers");
            DropTable("dbo.FimCustomerCarriers");
            DropTable("dbo.FimCarriers");
        }
    }
}
