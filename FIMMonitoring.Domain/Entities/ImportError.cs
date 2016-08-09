using System;
using System.ComponentModel.DataAnnotations;
using FIMMonitoring.Domain.Enum;

namespace FIMMonitoring.Domain
{
    public class ImportError : EntityBase
    {

        #region Properties

        public ErrorSource ErrorSource { get; set; }
        public ErrorLevel ErrorLevel { get; set; }
        public ErrorType? ErrorType { get; set; }
        public DateTime? ErrorDate { get; set; }
        public DateTime? ErrorSendDate { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool Cleared { get; set; }
        public bool IsValidated { get; set; }
        public bool IsParsed { get; set; }
        public bool IsDownloaded { get; set; }
        public bool IsChecked { get; set; }
        public bool IsBusinessValidated { get; set; }
        [MaxLength]
        public string Description { get; set; }
        public int SystemId { get; set; }
        [MaxLength(200)]
        public string Filename { get; set; }

        //id of file in SoftLogs
        public int? ImportId { get; set; }

        #endregion Properties

        #region ForeignKeys


        public int? FimImportSourceId { get; set; }
        public int? FimCustomerId { get; set; }
        public int? FimCarrierId { get; set; }
        #endregion ForeignKeys

        #region Virtuals
        public virtual FimSystem System { get; set; }
        public virtual FimImportSource  FimImportSource { get; set; }
        public virtual FimCustomer FimCustomer { get; set; }
        public virtual FimCarrier FimCarrier { get; set; }
        #endregion Virtuals
    }
}
