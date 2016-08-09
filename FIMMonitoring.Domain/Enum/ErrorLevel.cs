using System.ComponentModel.DataAnnotations;

namespace FIMMonitoring.Domain.Enum
{
    public enum ErrorLevel
    {
        [Display(Name = "None")]
        None = 0,
        [Display(Name = "Critical")]
        Critical = 1,
        [Display(Name = "Warning")]
        Warning = 2
    }
}
