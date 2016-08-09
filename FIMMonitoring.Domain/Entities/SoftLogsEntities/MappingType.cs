using System.ComponentModel.DataAnnotations;

namespace FIMMonitoring.Domain
{
    public class MappingType : EntityBase
    {
        [Required]
        public string MappingTypeName { get; set; }

        [Required]
        public string MappingClassName { get; set; }

        [Required]
        [StringLength(100)]
        public string Description { get; set; }

        public string Namespace { get; set; }

        public bool IsInternal { get; set; }
    }
}
