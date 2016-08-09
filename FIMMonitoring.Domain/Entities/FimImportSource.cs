using System.Collections.Generic;

namespace FIMMonitoring.Domain
{
    public class FimImportSource : ForeignEntitty
    {
        public int CarrierId { get; set; }

        public bool Disabled { get; set; }

        public virtual FimCarrier Carrier{ get; set; }

        public virtual List<ImportError> ImportErrors { get; set; }
    }
}
