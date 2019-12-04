using _2PricingCalculator.Backend.DAL;
using _2PricingCalculator.Backend.Model;
using System;
using System.Linq;

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

            decimal value = 0.0M;

            var customerServiceUsingInfos = pricingRepository.GetCustomerServicesUsing(request.CustomerId);
            // any service for customer?
            if (customerServiceUsingInfos?.Count > 0)
            {
                foreach (var customerServiceUsingInfo in customerServiceUsingInfos)
                {
                    var serviceType = customerServiceUsingInfo.ServiceType;
                    var customerId = customerServiceUsingInfo.CustomerId;

                    var serviceStartDate = customerServiceUsingInfo.StartDate;
                    var today = DateTime.Now.Date;
                    if (request.StartDate > today)
                    {
                        // start date from future
                        continue;
                    }

                    if (serviceStartDate > request.EndDate)
                    {
                        // services start after requested period
                        continue;
                    }

                    int freeDays = pricingRepository.GetFreeDays(customerId);
                    var servicePaymentStartDate = serviceStartDate.AddDays(freeDays);

                    if (servicePaymentStartDate > today)
                    {
                        // service payment start date from future
                        continue;
                    }

                    if (servicePaymentStartDate > request.EndDate)
                    {
                        // service pay days start after request period
                        continue;
                    }

                    var basePricing = pricingRepository.GetServicePricingInfo(serviceType);
                    var customerPricing = pricingRepository.GetCustomerServicePricingInfo(customerId, serviceType);

                    var dayCost = customerPricing != null ? customerPricing.CustomBaseCost : basePricing.BaseCost;

                    // find startDate
                    var startDate = request.StartDate > servicePaymentStartDate ? request.StartDate : servicePaymentStartDate;

                    // find endDate
                    var endDate = request.EndDate < today ? request.EndDate : today;

                    var discountsAll = pricingRepository.GetCustomerServiceDiscounts(customerId, serviceType);

                    var discountsForPeriod = discountsAll
                        .Where(d => (d.StartDate < endDate && (!d.EndDate.HasValue || d.EndDate.Value >= startDate)))
                        .Select(d => new
                        {
                            StartDate = d.StartDate < startDate ? startDate : d.StartDate,
                            EndDate = !d.EndDate.HasValue || d.EndDate.Value > endDate ? endDate : d.EndDate,
                            Value = d.Percent * 0.01M
                        })
                        .ToList();

                    while (startDate <= endDate)
                    {
                        var dayOfWeek = ((int)startDate.DayOfWeek) + 1;
                        if (basePricing.WorkdaysForPricing.Any(d => d == dayOfWeek))
                        {
                            decimal percent = 1.0M;
                            var discount = discountsForPeriod.FirstOrDefault(d => startDate >= d.StartDate && startDate <= d.EndDate);
                            if (discount != null)
                            {
                                percent = percent - discount.Value;
                            }

                            var dayValue = dayCost * percent;
                            value += dayValue;
                        }

                        startDate = startDate.AddDays(1);
                    }
                }
            }

            return new PriceCalculationResult { Value = value };
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