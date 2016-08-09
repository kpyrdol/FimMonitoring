using System.ComponentModel.DataAnnotations;

namespace FIMMonitoring.Domain.Enum
{
    public enum ErrorSource
    {
        [Display(Name="From import table")]
        ImportTable = 0,
        [Display(Name = "From log file")]
        LogFile = 1
    }
}
