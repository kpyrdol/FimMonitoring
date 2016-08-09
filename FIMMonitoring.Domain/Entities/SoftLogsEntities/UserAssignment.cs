using System.ComponentModel.DataAnnotations;

namespace FIMMonitoring.Domain
{
    public class UserAssignment : EntityBase
    {
        [StringLength(128)]
        [Required]
        public string UserId { get; set; }

        public int CustomerId { get; set; }

        public virtual Customer Customer { get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}
