using System;

namespace _2PricingCalculator.Backend.Model
{
    public class CustomerServiceDiscountInfo
    {
        public string ServiceType { get; set; }

        public string CustomerId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public decimal Percent { get; set; }
    }
}