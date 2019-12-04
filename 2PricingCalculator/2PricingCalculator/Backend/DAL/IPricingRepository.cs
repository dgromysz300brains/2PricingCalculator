using _2PricingCalculator.Backend.Model;
using System.Collections.Generic;

namespace _2PricingCalculator.Backend.DAL
{
    public interface IPricingRepository
    {
        List<CustomerServiceUsingInfo> GetCustomerServicesUsing(string customerId);

        ServicePricingInfo GetServicePricingInfo(string serviceType);

        CustomerServicePricingInfo GetCustomerServicePricingInfo(string customerId, string serviceType);

        int GetFreeDays(string customerId);

        List<CustomerServiceDiscountInfo> GetCustomerServiceDiscounts(string customerId, string serviceType);
    }
}