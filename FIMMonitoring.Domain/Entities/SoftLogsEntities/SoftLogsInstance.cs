using System.ComponentModel.DataAnnotations;

namespace FIMMonitoring.Domain
{
    public class SoftLogsInstance : EntityBase
    {
        [StringLength(200)]
        public string ContextClassName { get; set; }

        [StringLength(200)]
        public string Name { get; set; }
    }
}
