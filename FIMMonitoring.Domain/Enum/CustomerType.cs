using System.ComponentModel.DataAnnotations;

namespace FIMMonitoring.Domain
{
    public enum CustomerType
    {
        [Display(Name = "Full")]
        Full = 0,

        [Display(Name = "Light")]
        Light = 1,

        [Display(Name = "TPL")]
        Tpl = 2
    }
}
