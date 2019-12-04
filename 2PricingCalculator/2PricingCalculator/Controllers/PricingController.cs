using _2PricingCalculator.Backend.BLL;
using Microsoft.AspNetCore.Mvc;
using System;

namespace _2PricingCalculator.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PricingController : Controller
    {
        private readonly IPricingService pricingService;

        public PricingController(IPricingService pricingService)
        {
            this.pricingService = pricingService ?? throw new ArgumentNullException(nameof(pricingService));
        }

        [HttpGet("{id}")]
        public ActionResult<decimal> Calculate(string id, DateTime start, DateTime end)
        {
            var result = this.pricingService.CalculatePrice(new Backend.Model.PriceCalculateRequest
            {
                CustomerId = id,
                StartDate = start,
                EndDate = end
            });

            return result.Value;
        }
    }
}