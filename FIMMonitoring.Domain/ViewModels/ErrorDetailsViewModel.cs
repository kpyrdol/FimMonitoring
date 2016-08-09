using System;
using FIMMonitoring.Domain.Enum;

namespace FIMMonitoring.Domain.ViewModels
{
    public class ErrorDetailsViewModel
    {
        public int Id { get; set; }
        public int? ImportId { get; set; }
        public string SystemUrl { get; set; }
        public bool IsValidated { get; set; }
        public bool IsParsed { get; set; }
        public bool IsDownloaded { get; set; }
        public bool IsCleared { get; set; }
        public string SourceName { get; set; }
        public DateTime? ErorrDate { get; set; }
        public DateTime? SendErorrDate { get; set; }
        public ErrorLevel ErrorLevel { get; set; }
        public ErrorType? ErrorType { get; set; }
        public DateTime ProcessedDate { get; set; }
    }
}
