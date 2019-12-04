using System.Collections.Generic;

namespace _2PricingCalculator.Backend.Model
{
    public class ServicePricingInfo
    {
        public string ServiceType { get; set; }

        public decimal BaseCost { get; set; }

        public List<int> WorkdaysForPricing { get; set; }
    }
}