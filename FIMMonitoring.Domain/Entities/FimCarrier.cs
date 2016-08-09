using System.Collections.Generic;

namespace FIMMonitoring.Domain
{
    public class FimCarrier : ForeignEntitty
    {
        public virtual List<FimCustomerCarrier> CustomerCarriers { get; set; }
        public virtual List<FimImportSource> FimImportSources { get; set; }
        public virtual List<ImportError> ImportErrors{ get; set; }
    }
}
