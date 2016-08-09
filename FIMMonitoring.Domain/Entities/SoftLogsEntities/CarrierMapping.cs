using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace FIMMonitoring.Domain
{
    public class CarrierMapping : EntityBase
    {
        public virtual Carrier Carrier { get; set; }

        public int CarrierId { get; set; }

        public virtual MappingType MappingType { get; set; }

        public virtual ICollection<ImportSource> ImportSources { get; set; }

        public int MappingTypeId { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
