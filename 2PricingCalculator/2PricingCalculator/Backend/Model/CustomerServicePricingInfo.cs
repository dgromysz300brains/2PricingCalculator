namespace _2PricingCalculator.Backend.Model
{
    public class CustomerServicePricingInfo
    {
        public string CustomerId { get; set; }

        public string ServiceType { get; set; }

        public decimal CustomBaseCost { get; set; }
    }
}