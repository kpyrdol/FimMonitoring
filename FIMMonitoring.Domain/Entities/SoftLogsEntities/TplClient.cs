using System.ComponentModel.DataAnnotations;

namespace FIMMonitoring.Domain.Entities
{
    public class TplClient : EntityBase
    {
        public int? CustomerId { get; set; }

        [StringLength(100)]
        [Required]
        public string Name { get; set; }

        [StringLength(100)]
        public string Description { get; set; }

        [StringLength(100)]
        public string EmailAddress { get; set; }

        public bool IsEnabled { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
