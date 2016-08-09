using System.Collections.Generic;

namespace FIMMonitoring.Domain
{
    public class CustomerViewModel
    {
        public CustomerViewModel()
        {
            Carriers = new List<CarrierDetailsViewModel>();
        }

        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string SystemName { get; set; }

        public List<CarrierDetailsViewModel> Carriers { get; set; } 
    }
}
