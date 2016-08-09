using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FIMMonitoring.Domain
{
    public class Import : EntityBase
    {
        public DateTime? CreatedAt { get; set; }

        [StringLength(10)]
        public string Encoding { get; set; }

        [Required]
        public string Filename { get; set; }

        public virtual ImportSource ImportSource { get; set; }

        public int ImportSourceID { get; set; }

        public virtual MappingType MappingType { get; set; }

        public int MappingTypeId { get; set; }

        [ForeignKey("RawContentBlobID")]
        public virtual Blob RawContent { get; set; }

        public Guid RawContentBlobID { get; set; }

        [ForeignKey("DocumentContentBlobID")]
        public virtual Blob DocumentContent { get; set; }

        public Guid? DocumentContentBlobID { get; set; }

        public string DocumentContentName { get; set; }

        public string DocumentContentMimeType { get; set; }

        public ImportDataType ImportDataType { get; set; }

        [NotMapped]
        public bool IsSelected { get; set; }

        public bool IsDownloaded { get; set; }

        public bool IsValidated { get; set; }

        public bool IsParsed { get; set; }
        
        public bool IsChecked { get; set; }

        public bool IsFinished { get; set; }
    }
}
