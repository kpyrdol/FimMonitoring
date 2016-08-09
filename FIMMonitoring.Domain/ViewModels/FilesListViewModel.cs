using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using FIMMonitoring.Domain.Enum;
using PagedList;

namespace FIMMonitoring.Domain
{
    public class FilesListViewModel
    {

        public FilesListViewModel()
        {
            SelectedSources = new List<int>();
            ImportSources = new List<ImportSourceViewModel>();
            ErrorLevels = new List<ErrorLevel>();
            SourceErrors = new List<SourceError>();
        }

        public string SystemName { get; set; }
        public int CarrierId { get; set; }
        public string CarrierName { get; set; }

        public int? Page { get; set; }
        public int PageSize { get; set; }

        public int CustomerId { get; set; }
        public string CustomerName { get; set; }

        public List<int> SelectedSources { get; set; }

        public IPagedList<FilesListItemViewModel> Files { get; set; }

        public List<ImportSourceViewModel> ImportSources { get; set; }

        public List<ErrorLevel> ErrorLevels { get; set; }

        public List<SourceError> SourceErrors { get; set; }

        public string GetState(int sourceId, ErrorLevel error)
        {
            return SourceErrors.Any(e => e.SourceId == sourceId && e.ErrorLevels.Any(er=>er == error)) ? "on" : "off";
        }
    }

    public class SourceError
    {
        public int SourceId { get; set; }
        public List<ErrorLevel> ErrorLevels { get; set; }
    }

    public class FilesListItemViewModel
    {
        public int Id { get; set; }
        public int? ImportId { get; set; }
        public bool IsDownloaded { get; set; }
        public bool IsParsed { get; set; }
        public bool IsValidated { get; set; }
        public bool IsCleared { get; set; }

        public int SystemId { get; set; }
        public string SystemName { get; set; }
        public string SystemUrl { get; set; }

        public string Description { get; set; }
        public ErrorSource ErrorSource { get; set; }

        [Display(Name = "Error occurrence")]
        public DateTime? ErrorDate { get; set; }
        [Display(Name="Processed date")]
        public DateTime ProcessedDate { get; set; }
        [Display(Name = "Error notification send")]
        public DateTime? ErrorSendDateTime { get; set; }
        [Display(Name = "Error level")]
        public ErrorLevel ErrorLevel { get; set; }
        [Display(Name = "Error type")]
        public ErrorType? ErrorType { get; set; }
        [Display(Name="Source name")]
        public string SourceName { get; set; }

        public bool SourceDisabled { get; set; }
    }
}
