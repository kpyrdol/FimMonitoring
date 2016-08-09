using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIMMonitoring.Domain
{
    public class UploadedReport : EntityBase
    {
        public Guid Identifier { get; set; }

        public int CarrierId { get; set; }

        public int CustomerId { get; set; }

        public int Month { get; set; }

        public int Year { get; set; }

        public DateTime DownloadedAt { get; set; }

        //[ForeignKey("DownloadedBy")]
        //public string DownloadedById { get; set; }

        public DateTime? UploadedAt { get; set; }

        //[ForeignKey("UploadedBy")]
        //public string UploadedById { get; set; }

        public bool HasGroupedWaybills { get; set; }

        public virtual Carrier Carrier { get; set; }

        public virtual Customer Customer { get; set; }

        //public virtual ApplicationUser DownloadedBy { get; set; }

        //public virtual ApplicationUser UploadedBy { get; set; }
    }
}
