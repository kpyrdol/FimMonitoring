using FIMMonitoring.Domain.Enum;
using System;
using System.ComponentModel.DataAnnotations;

namespace FIMMonitoring.Domain
{
    public class ServiceImportError : EntityBase
    {

        #region Properties

        public ErrorSource ErrorSource { get; set; }
        public ErrorLevel ErrorLevel { get; set; }
        public ErrorType? ErrorType { get; set; }
        public DateTime? ErrorDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsValidated { get; set; }
        public bool IsParsed { get; set; }
        public bool IsDownloaded { get; set; }
        public bool IsSent { get; set; }
        public bool IsBusinessValidated { get; set; }
        [MaxLength]
        public string Description { get; set; }

        [MaxLength(200)]
        public string CustomerName { get; set; }
        [MaxLength(200)]
        public string CarrierName { get; set; }
        [MaxLength(200)]
        public string SourceName { get; set; }

        public Guid Guid { get; set; }    
        #endregion Properties

        #region ForeignKeys

        public int? ImportId { get; set; }
        public int? SourceId { get; set; }
        public int? CustomerId { get; set; }
        public int? CarrierId { get; set; }
        public int? FileCheckId { get; set; }
        #endregion ForeignKeys

        #region Virtuals
        public virtual FileCheck FileCheck { get; set; }
        #endregion Virtuals
    }
}
