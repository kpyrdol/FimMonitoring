using System;
using System.ComponentModel.DataAnnotations;

namespace FIMMonitoring.Domain
{
    public class FileCheck : EntityBase
    {
        [StringLength(200)]
        public string FileName { get; set; }
        public bool Parsed { get; set; }
        public DateTime ParseDate { get; set; }
    }
}
