using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FIMMonitoring.Domain.Entities;

namespace FIMMonitoring.Domain
{
    public class Customer : EntityBase
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public CustomerType CustomerType { get; set; }

        public bool IsEnabled { get; set; }

        [StringLength(5)]
        [Required]
        public string Country { get; set; }

        public virtual byte[] Logo { get; set; }

        [StringLength(50)]
        public string LogoContentType { get; set; }

        public string HomepageUrl { get; set; }

        [ForeignKey("AssociatedCustomer")]
        public int? AssociatedCustomerId { get; set; }

        public virtual Customer AssociatedCustomer { get; set; }

        public bool? IsTplSales { get; set; }

        public bool UseAutomaticAnalysis { get; set; }

        public virtual ICollection<CustomerCarrier> CustomerCarriers { get; set; }

        public virtual ICollection<TplClient> CustomerClients { get; set; }

        public virtual ICollection<ImportSource> ImportSources { get; set; }
    }
}
