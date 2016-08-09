using System.Collections.Generic;

namespace FIMMonitoring.Domain
{
    public class FimCustomer : ForeignEntitty
    {
        public virtual List<FimCustomerCarrier> FimCustomerCarriers { get; set; }

        public virtual List<ImportError> ImportErrors { get; set; }
    }
}
