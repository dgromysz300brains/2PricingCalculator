using _2PricingCalculator.Backend.Model;

namespace _2PricingCalculator.Backend.BLL
{
    public interface IPricingService
    {
        PriceCalculationResult CalculatePrice(PriceCalculateRequest request);
    }
}