using Microsoft.Extensions.Configuration;
using Polly;
using PricingService.Api.Commands;
using RestEase;
using System;
using System.Net.Http;
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

        private static Policy retryPolicy = Policy
            .Handle<HttpRequestException>()
            .WaitAndRetryAsync(retryCount: 3, sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(3));

        public PricingClient(IConfiguration configuration)
        {
            restEasyClient = RestClient.For<IPricingClient>(configuration.GetValue<string>("PricingServiceUri"));
        }

        public Task<CalculatePriceResult> CalculatePrice([Body] CalculatePriceCommand cmd)
        {
            return retryPolicy.ExecuteAsync(async () => await this.restEasyClient.CalculatePrice(cmd));
        }
    }
}
