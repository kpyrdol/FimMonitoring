using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FIMMonitoring.Domain
{
    public class Invoice : EntityBase
    {
        [StringLength(50)]
        public string InvoiceNumber { get; set; }

        public decimal NetAmount { get; set; }

        public decimal ValidatedNetAmount { get; set; }

        [StringLength(50)]
        public string ReceiverNumber { get; set; }

        public virtual Import Import { get; set; }

        public int? ImportID { get; set; }

        public virtual ICollection<Waybill> Waybills { get; set; }

        public ValidationModes ValidationMode { get; set; }


    }
}
