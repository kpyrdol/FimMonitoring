namespace FIMMonitoring.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ImportErrorsMig : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.ImportErrors",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        ErrorLevel = c.Int(nullable: false),
                        ErrorType = c.Int(),
                        ErrorDate = c.DateTime(),
                        ErrorsSendDate = c.DateTime(),
                        CreatedAt = c.DateTime(nullable: false),
                        Cleared = c.Boolean(nullable: false),
                        IsValidated = c.Boolean(nullable: false),
                        IsParsed = c.Boolean(nullable: false),
                        IsDownloaded = c.Boolean(nullable: false),
                        ImportId = c.Int(nullable: false),
                        SourceId = c.Int(nullable: false),
                        CustomerId = c.Int(nullable: false),
                        CarrierId = c.Int(nullable: false),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.ImportErrors");
        }
    }
}
