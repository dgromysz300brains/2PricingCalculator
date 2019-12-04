using _2PricingCalculator.Backend.Model;
using System;
using System.Collections.Generic;
using System.Linq;

namespace _2PricingCalculator.Backend.DAL
{
    public class MockPricingRepository : IPricingRepository
    {
        private const string SERVICE_A = "A";
        private const string SERVICE_B = "B";
        private const string SERVICE_C = "C";

        private Dictionary<string, ServicePricingInfo> _servicePricingInfo = new Dictionary<string, ServicePricingInfo>
        {
            { SERVICE_A, new ServicePricingInfo { ServiceType = SERVICE_A, BaseCost = 0.2M, WorkdaysForPricing = new List<int> { 1, 2, 3, 4, 5 } } },
            { SERVICE_B, new ServicePricingInfo { ServiceType = SERVICE_B, BaseCost = 0.24M, WorkdaysForPricing = new List<int> { 1, 2, 3, 4, 5 } } },
            { SERVICE_C, new ServicePricingInfo { ServiceType = SERVICE_C, BaseCost = 0.4M, WorkdaysForPricing = new List<int> { 1, 2, 3, 4, 5, 6, 7 } } }
        };

        private List<CustomerServicePricingInfo> _customerServicePricingInfos = new List<CustomerServicePricingInfo>
        {
            new CustomerServicePricingInfo { ServiceType = SERVICE_A, CustomerId = "A", CustomBaseCost = 0.15M },
            new CustomerServicePricingInfo { ServiceType = SERVICE_B, CustomerId = "A", CustomBaseCost = 0.25M }
        };

        private List<CustomerServiceUsingInfo> _customerServiceUsingInfo = new List<CustomerServiceUsingInfo>
        {
            new CustomerServiceUsingInfo { ServiceType = SERVICE_A, CustomerId = "1", StartDate = new DateTime(2019, 1, 1) },
            new CustomerServiceUsingInfo { ServiceType = SERVICE_B, CustomerId = "2", StartDate = new DateTime(2019, 5, 15) },
            new CustomerServiceUsingInfo { ServiceType = SERVICE_A, CustomerId = "X", StartDate = new DateTime(2019, 9, 20) },
            new CustomerServiceUsingInfo { ServiceType = SERVICE_C, CustomerId = "X", StartDate = new DateTime(2019, 9, 20) },
            new CustomerServiceUsingInfo { ServiceType = SERVICE_B, CustomerId = "Y", StartDate = new DateTime(2018, 1, 1) },
            new CustomerServiceUsingInfo { ServiceType = SERVICE_C, CustomerId = "Y", StartDate = new DateTime(2018, 1, 1) }
        };

        private Dictionary<string, int> _customersFreeDays = new Dictionary<string, int>
        {
            { "1", 0 },
            { "2", 5 },
            { "X", 0 },
            { "Y", 200 }
        };

        private List<CustomerServiceDiscountInfo> _customerServiceDiscountInfos = new List<CustomerServiceDiscountInfo>
        {
            new CustomerServiceDiscountInfo { ServiceType = SERVICE_C, CustomerId = "X", StartDate = new DateTime(2019, 9, 22), EndDate = new DateTime(2019, 9, 24), Percent = 20M  },
            new CustomerServiceDiscountInfo { ServiceType = SERVICE_B, CustomerId = "Y", StartDate = new DateTime(2018, 1, 1), EndDate = null, Percent = 20M  },
            new CustomerServiceDiscountInfo { ServiceType = SERVICE_C, CustomerId = "Y", StartDate = new DateTime(2018, 1, 1), EndDate = null, Percent = 30M  }
        };

        public CustomerServicePricingInfo GetCustomerServicePricingInfo(string customerId, string serviceType)
        {
            return _customerServicePricingInfos.FirstOrDefault(i =>
                string.Equals(i.ServiceType, serviceType, StringComparison.InvariantCultureIgnoreCase)
                && string.Equals(i.CustomerId, customerId, StringComparison.InvariantCultureIgnoreCase));
        }

        public int GetFreeDays(string customerId)
        {
            if (_customersFreeDays.TryGetValue(customerId, out int freeDays))
            {
                return freeDays;
            }

            return 0;
        }

        public ServicePricingInfo GetServicePricingInfo(string serviceType)
        {
            if (_servicePricingInfo.TryGetValue(serviceType, out var servicePricingInfo))
            {
                return servicePricingInfo;
            }

            throw new InvalidOperationException($"ServiceType {serviceType} not supported");
        }

        public List<CustomerServiceUsingInfo> GetCustomerServicesUsing(string customerId)
        {
            return _customerServiceUsingInfo.Where(i => string.Equals(i.CustomerId, customerId, StringComparison.InvariantCultureIgnoreCase))
                .ToList();
        }

        public List<CustomerServiceDiscountInfo> GetCustomerServiceDiscounts(string customerId, string serviceType)
        {
            return _customerServiceDiscountInfos.Where(i =>
                string.Equals(i.ServiceType, serviceType, StringComparison.InvariantCultureIgnoreCase)
                && string.Equals(i.CustomerId, customerId, StringComparison.InvariantCultureIgnoreCase))
                .ToList();
        }
    }
}