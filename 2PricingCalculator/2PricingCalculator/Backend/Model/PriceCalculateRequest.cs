using System;

namespace _2PricingCalculator.Backend.Model
{
    public class PriceCalculateRequest
    {
        public string CustomerId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}