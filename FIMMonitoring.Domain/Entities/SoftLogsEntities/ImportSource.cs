using System;
using System.ComponentModel.DataAnnotations;

namespace FIMMonitoring.Domain
{
    public class ImportSource : EntityBase
    {
        public ImportSource()
        {
            CheckInterval = 10;
        }

        [Required]
        [StringLength(240)]
        public string Address { get; set; }

        public virtual CarrierMapping CarrierMapping { get; set; }

        public int CarrierMappingId { get; set; }

        [Required]
        public int CheckInterval { get; set; }

        [Required]
        public CommunicationProtocols CommunicationProtocol { get; set; }

        public virtual Customer Customer { get; set; }

        public int CustomerId { get; set; }

        [StringLength(15)]
        public string Encoding { get; set; }

        public bool IsEnabled { get; set; }

        public DateTime? LastChecked { get; set; }

        [Required]
        public string Name { get; set; }

        public string Password { get; set; }

        public string Username { get; set; }

        public ValidationModes ValidationMode { get; set; }

        [StringLength(5)]
        public string FromCountry { get; set; }

        [StringLength(5)]
        public string ToCountry { get; set; }

        [StringLength(100)]
        public string TransportService { get; set; }

        public int? PostprocessingStepId { get; set; }

        public virtual PostprocessingStep PostprocessingStep { get; set; }
    }
}
