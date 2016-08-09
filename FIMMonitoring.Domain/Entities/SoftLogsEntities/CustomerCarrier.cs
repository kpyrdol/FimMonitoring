using System.ComponentModel.DataAnnotations;

namespace FIMMonitoring.Domain
{
    public class CustomerCarrier : EntityBase
    {
        public int CustomerId { get; set; }

        public int CarrierId { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual Carrier Carrier { get; set; }

        [StringLength(100)]
        public string EmailAddress { get; set; }

        [StringLength(10)]
        public string DeviationReportThreshold { get; set; }

        [StringLength(10)]
        public string DeviationChargesReportThreshold { get; set; }

        [StringLength(1)]
        public string DeviationReportPeriod { get; set; }
    }
}
