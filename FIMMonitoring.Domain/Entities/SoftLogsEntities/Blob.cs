using System;
using System.ComponentModel.DataAnnotations;

namespace FIMMonitoring.Domain
{
    public class Blob
    {
        public Blob()
        {
            BlobID = Guid.NewGuid();
        }

        [Key]
        public Guid BlobID { get; set; }

        [Required]
        public byte[] Content { get; set; }
    }
}
