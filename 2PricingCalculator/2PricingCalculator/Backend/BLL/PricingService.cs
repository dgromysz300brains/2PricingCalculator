using _2PricingCalculator.Backend.DAL;
using _2PricingCalculator.Backend.Model;
using System;

namespace _2PricingCalculator.Backend.BLL
{
    public class PricingService : IPricingService
    {
        private readonly IPricingRepository pricingRepository;

        public PricingService(IPricingRepository pricingRepository)
        {
            this.pricingRepository = pricingRepository ?? throw new System.ArgumentNullException(nameof(pricingRepository));
        }

        public PriceCalculationResult CalculatePrice(PriceCalculateRequest request)
        {
            ValidateRequest(request);

            return new PriceCalculationResult { Value = 0.01M };
        }

        private static void ValidateRequest(PriceCalculateRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException();
            }

            if (string.IsNullOrWhiteSpace(request.CustomerId))
            {
                throw new NullReferenceException(nameof(request.CustomerId));
            }

            if (request.StartDate > request.EndDate)
            {
                throw new InvalidProgramException($"StartDate can't be grater than EndDate");
            }
        }
    }
}