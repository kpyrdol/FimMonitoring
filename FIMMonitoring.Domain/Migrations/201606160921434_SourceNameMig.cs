namespace FIMMonitoring.Domain.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class SourceNameMig : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.ImportErrors", "SourceName", c => c.String(maxLength: 200));
        }
        
        public override void Down()
        {
            DropColumn("dbo.ImportErrors", "SourceName");
        }
    }
}
