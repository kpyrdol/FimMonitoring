using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FIMMonitoring.Domain
{
    public class Waybill : EntityBase
    {
        [StringLength(50)]
        public string WaybillNumber { get; set; }

        public decimal NetAmount { get; set; }

        [StringLength(200)]
        public string TransportService { get; set; }

        public virtual Invoice Invoice { get; set; }

        public int InvoiceID { get; set; }

        public virtual ICollection<Charge> Charges { get; set; }

    }
}
