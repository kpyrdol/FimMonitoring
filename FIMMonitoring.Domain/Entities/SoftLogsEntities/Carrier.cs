using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FIMMonitoring.Domain
{
    public class Carrier : EntityBase
    {
        public virtual ICollection<CustomerCarrier> CustomerCarriers { get; set; }
        public virtual ICollection<CarrierMapping> CarrierMappings { get; set; }

        public string HomepageUrl { get; set; }

        [StringLength(250)]
        public string TrackAndTraceUrl { get; set; }

        public bool IsEnabled { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        [StringLength(20)]
        public string ShortName { get; set; }

        public virtual byte[] Logo { get; set; }

        [StringLength(50)]
        public string LogoContentType { get; set; }
    }
}
