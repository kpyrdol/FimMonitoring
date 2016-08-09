using System.ComponentModel.DataAnnotations;

namespace FIMMonitoring.Domain.Enum
{
    public enum ErrorType
    {
        [Display(Name = "Can not reach source")]
        ReachSourceError = 0,
        [Display(Name = "Can not download file")]
        DownloadFileError = 1,
        [Display(Name = "Incorrect file format")]
        IncorrectFileFormatError = 2,
        [Display(Name = "Last file downloaded at")]
        //Od x dni nie pojawiło się nic nowego
        LongPeriodTimeError = 3,
        [Display(Name = "Missing data")]
        //brak konkretnych, kluczowych cech w plikach
        MissingDataError = 4,
        [Display(Name = "Data mismatch")]
        //Dane nie trzymają się kupoy, podobne do diff, ale bardziej rozwinięty
        DataMismatchError = 5

    }
}
