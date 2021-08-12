using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Polly;
using Polly.Retry;
using PricingService.Api.Commands;
using RestEase;
using Steeltoe.Common.Discovery;
using Steeltoe.Discovery;

namespace PolicyService.RestClients
{
    public interface IPricingClient
    {
        [Post]
        Task<CalculatePriceResult> CalculatePrice([Body] CalculatePriceCommand cmd);
    }

    public class PricingClient : IPricingClient
    {
        private readonly IPricingClient client;

        private static AsyncRetryPolicy retryPolicy = Policy
            .Handle<HttpRequestException>()
            .WaitAndRetryAsync(retryCount: 3, sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(3));

        public PricingClient(IConfiguration configuration, IDiscoveryClient discoveryClient)
        {
            var handler = new DiscoveryHttpClientHandler(discoveryClient);
            var httpClient = new HttpClient(handler, false)
            {
                BaseAddress = new Uri(configuration.GetValue<string>("PricingServiceUri"))
            };
            client = RestClient.For<IPricingClient>(httpClient);
        }

        public Task<CalculatePriceResult> CalculatePrice([Body] CalculatePriceCommand cmd)
        {
            return retryPolicy.ExecuteAsync(async () => await client.CalculatePrice(cmd));
        }
    }
}
