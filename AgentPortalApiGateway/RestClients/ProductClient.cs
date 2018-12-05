using ProductService.Api.Queries.Dtos;
using RestEase;
using Polly;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System;
using Microsoft.Extensions.Configuration;

namespace AgentPortalApiGateway.RestClients
{
    public interface IProductClient
    {
        [Get]
        Task<IEnumerable<ProductDto>> GetAll();

        [Get("/{code}")]
        Task<ProductDto> GetByCode([Path] string code);
    }

    public class ProductClient : IProductClient
    {
        private readonly IProductClient client;

        private static Policy retryPolicy = Policy
            .Handle<HttpRequestException>()
            .WaitAndRetryAsync(retryCount: 3, sleepDurationProvider: retryAttempt => TimeSpan.FromSeconds(3));

        public ProductClient(IConfiguration configuration)
        {
            client = RestClient.For<IProductClient>(configuration.GetValue<string>("ProductServiceUri"));
        }

        public Task<IEnumerable<ProductDto>> GetAll()
        {
            return retryPolicy.ExecuteAsync(async () => await client.GetAll());
        }

        public Task<ProductDto> GetByCode([Path] string code)
        {
            return retryPolicy.ExecuteAsync(async () => await client.GetByCode(code));
        }
    }
}
