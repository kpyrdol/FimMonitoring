using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Diagnostics;
using FIMMonitoring.Domain.Entities;

namespace FIMMonitoring.Domain
{
    public class SoftLogsContext : DbContext
    {
        public SoftLogsContext(int commandTimeout)
            : base("Name=SoftLogsDB")
        {
            Database.SetInitializer<SoftLogsContext>(null);
            ((IObjectContextAdapter)this).ObjectContext.CommandTimeout = commandTimeout;

#if DEBUG
            Database.Log = msg => Debug.WriteLine(msg);
#endif
        }

        public SoftLogsContext()
            : this(90)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Properties<decimal>().Configure(config => config.HasPrecision(14, 4));
            modelBuilder.Properties<string>().Configure(config => config.HasMaxLength(100));
        }


        public DbSet<Carrier> Carriers { get; set; }
        public DbSet<CarrierMapping> CarrierMappings { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<CustomerCarrier> CustomerCarriers { get; set; }
        public DbSet<ImportSource> ImportSources { get; set; }
        public DbSet<PostprocessingStep> PostprocessingSteps { get; set; }
        public DbSet<TplClient> TplClients { get; set; }
        public DbSet<Blob> Blobs { get; set; }
        public DbSet<Import> Imports { get; set; }
        public DbSet<UploadedReport> UploadedReports { get; set; }
        public DbSet<Charge> Charges{ get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<Waybill> Waybills { get; set; }

        public DataTable GetValidationErrors(int id)
        {
            var cmd = Database.Connection.CreateCommand();

            cmd.CommandType = CommandType.Text;
            cmd.CommandText = string.Format("select * from dbo.getInvalidValues({0})", id);
            var dt = new DataTable("ExportedRows");
            if (cmd.Connection.State != ConnectionState.Open)
                cmd.Connection.Open();
            using (var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection))
            {
                dt.Load(reader, LoadOption.OverwriteChanges);
            }

            return dt;
        }

        public IEnumerable<string> GetInvoiceNumbersByImport(int id)
        {
            return Database.SqlQuery<string>("exec dbo.getInvoiceNumbersByImport @p0", id);
        }

        public void DeleteImportedData(int? importID, int? customerId, int? carrierId)
        {
            Database.ExecuteSqlCommand("exec dbo.clearImportedData @p0,@p1,@p2", importID, customerId, carrierId);
        }
    }



}
