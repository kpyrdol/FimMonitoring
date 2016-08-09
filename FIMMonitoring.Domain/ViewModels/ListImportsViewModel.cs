using System.Collections.Generic;
using System.Linq;

namespace FIMMonitoring.Domain
{
    public class ListImportsViewModel
    {
        public ListImportsViewModel()
        {
            Systems = new List<SystemViewModel>();
            Customers = new List<CustomerImportViewModel>();
        }
        public List<SystemViewModel> Systems { get; set; }
        public List<CustomerImportViewModel> Customers { get; set; }
    }

    public class SystemViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public SystemViewModel()
        {
            Customers = new List<CustomerImportViewModel>();
        }

        public List<CustomerImportViewModel> Customers { get; set; }
    }

    public class CustomerImportViewModel
    {
        public CustomerImportViewModel()
        {
            Carriers = new List<CarrierViewModel>();
        }

        public int CustomerId { get; set; }
        public string CustomerName { get; set; }

        public string SystemCss { get; set; }

        public List<CarrierViewModel> Carriers { get; set; }

        public bool IsValid => Carriers.All(e => e.IsValid);
    }

    public class CarrierDetailsViewModel
    {
        public CarrierDetailsViewModel()
        {
            Sources = new List<ImportSourceViewModel>();
        }
        public string Name { get; set; }
        public int CarrierId { get; set; }

        public List<ImportSourceViewModel> Sources { get; set; }

        public int CountErrors { get; set; }
        public int CountWarnings { get; set; }
        public int CountOk { get; set; }

        public int CountErrorsCleared { get; set; }
        public int CountWarningsCleared { get; set; }
        public int CountOkCleared { get; set; }

        public string CountErrorsString => $"{CountErrors}/{CountErrorsCleared}";
        public string CountWarningsString => $"{CountWarnings}/{CountWarningsCleared}";
        public string CountOkString => $"{CountOk}";

        public int CountErrorsNotDisabled { get; set; }
        public int CountWarningsNotDisabled { get; set; }

        public bool IsValid => Sources.All(e => e.IsValid);
    }

    public class CarrierViewModel
    {
        public string Name { get; set; }
        public int CarrierId { get; set; }

        public int CountErrors { get; set; }
        public int CountWarnings { get; set; }
        public int CountOk { get; set; }

        public int CountErrorsCleared { get; set; }
        public int CountWarningsCleared { get; set; }

        public string CountErrorsString => $"{CountErrors}/{CountErrorsCleared}";
        public string CountWarningsString => $"{CountWarnings}/{CountWarningsCleared}";
        public string CountOkString => $"{CountOk}";

        public bool IsValid => CountErrors - CountErrorsNotDisabled == 0 && CountWarnings == 0;
        public int CountErrorsNotDisabled { get; set; }
        public int CountWarningsNotDisabled { get; set; }
        public bool Disabled { get; set; }
    }

    public class ImportSourceViewModel
    {

        public bool Checked { get; set; }
        public bool Enabled { get; set; }
        public string Name { get; set; }
        public int SourceId { get; set; }
        public int CarrierId { get; set; }
        public int CustomerId { get; set; }

        public int CountErrors { get; set; }
        public int CountWarnings { get; set; }
        public int CountOk { get; set; }

        public int CountErrorsCleared { get; set; }
        public int CountWarningsCleared { get; set; }
        public int CountOkCleared { get; set; }

        public string CountErrorsString => $"{CountErrors}/{CountErrorsCleared}";
        public string CountWarningsString => $"{CountWarnings}/{CountWarningsCleared}";
        public string CountOkString => $"{CountOk}";

        public bool Disabled { get; set; }

        public int CountErrorsNotDisabled { get; set; }
        public int CountWarningsNotDisabled { get; set; }


        public bool IsValid => CountErrors == 0 && CountWarnings == 0;


    }

}
