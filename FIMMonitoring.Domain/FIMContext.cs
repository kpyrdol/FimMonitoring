using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Diagnostics;
using Microsoft.AspNet.Identity.EntityFramework;

namespace FIMMonitoring.Domain
{
    public class FIMContext : IdentityDbContext<ApplicationUser>
    {
        public FIMContext(int commandTimeout)
            : base("Name=FimDB")
        {
            ((IObjectContextAdapter) this).ObjectContext.CommandTimeout = commandTimeout;

#if DEBUG
            Database.Log = msg => Debug.WriteLine(msg);
#endif
        }

        public FIMContext()
            : this(90)
        {
        }

        public DbSet<ImportError> ImportErrors { get; set; }
        public DbSet<ServiceImportError> ServiceImportErrors { get; set; }
        public DbSet<FileCheck> FileChecks { get; set; }
        public DbSet<FimCustomer> FimCustomers { get; set; }
        public DbSet<FimImportSource> FimImportSources { get; set; }
        public DbSet<FimCarrier> FimCarriers { get; set; }
        public DbSet<FimSystem> FimSystems { get; set; }
        public DbSet<FimCustomerCarrier> FimCustomerCarriers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<ManyToManyCascadeDeleteConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
            modelBuilder.Properties<decimal>().Configure(config => config.HasPrecision(14, 4));
            modelBuilder.Properties<string>().Configure(config => config.HasMaxLength(100));
        }
    }
}
