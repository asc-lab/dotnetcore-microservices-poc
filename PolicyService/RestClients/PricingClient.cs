using Microsoft.Extensions.Configuration;
using PricingService.Api.Commands;
using RestEase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PolicyService.RestClients
{
    public interface IPricingClient
    {
        [Post]
        Task<CalculatePriceResult> CalculatePrice([Body] CalculatePriceCommand cmd);
    }

    public class PricingClient : IPricingClient
    {
        private readonly IPricingClient restEasyClient;

        public PricingClient(IConfiguration configuration)
        {
            this.restEasyClient = RestClient.For<IPricingClient>(configuration.GetValue<string>("PricingServiceUri"));
        }

        public Task<CalculatePriceResult> CalculatePrice([Body] CalculatePriceCommand cmd)
        {
            return this.restEasyClient.CalculatePrice(cmd);
        }
    }
}
