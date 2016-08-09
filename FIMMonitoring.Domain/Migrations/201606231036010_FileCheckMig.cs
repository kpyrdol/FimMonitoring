namespace FIMMonitoring.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FileCheckMig : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.FileChecks",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FileName = c.String(maxLength: 100),
                        Parsed = c.String(maxLength: 100),
                        ParseDate = c.DateTime(nullable: false),
                        Timestamp = c.Binary(nullable: false, fixedLength: true, timestamp: true, storeType: "rowversion"),
                    })
                .PrimaryKey(t => t.Id);
            
            AddColumn("dbo.ImportErrors", "ErrorSource", c => c.Int(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ImportErrors", "ErrorSource");
            DropTable("dbo.FileChecks");
        }
    }
}
