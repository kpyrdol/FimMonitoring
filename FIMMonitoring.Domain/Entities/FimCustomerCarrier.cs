namespace FIMMonitoring.Domain
{
    public class FimCustomerCarrier : EntityBase
    {
        public int FimCustomerId { get; set; }

        public int FimCarrierId { get; set; }

        public virtual FimCustomer FimCustomer { get; set; }

        public virtual FimCarrier FimCarrier { get; set; }
    }
}
