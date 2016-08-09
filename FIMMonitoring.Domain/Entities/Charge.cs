using System.ComponentModel.DataAnnotations;

namespace FIMMonitoring.Domain
{
    public class Charge : EntityBase
    {

        public decimal? CalculatedValue { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        [StringLength(100)]
        public string MappedCode { get; set; }

        public decimal TotalAmount { get; set; }

        public virtual Waybill Waybill { get; set; }

        public int WaybillID { get; set; }
    }
}
