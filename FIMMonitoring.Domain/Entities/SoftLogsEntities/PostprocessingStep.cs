using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FIMMonitoring.Domain
{
    public class PostprocessingStep : EntityBase
    {
        [Required]
        public string Name { get; set; }

        [StringLength(500)]
        public string Description { get; set; }

        [Required]
        public string ProcedureName { get; set; }

        public virtual ICollection<ImportSource> ImportSources { get; set; }
    }
}
