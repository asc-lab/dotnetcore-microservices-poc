using AgentPortalUi.BlazorWasm.Contracts;
using AgentPortalUi.BlazorWasm.Contracts.Dto;
using RestEase;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace AgentPortalUi.BlazorWasm.Services
{
    public class ProductsService : IProductsService
    {
        private readonly IProductsService client;

        public ProductsService(UserService userService)
        {
            var httpClient = new HttpClient()
            {
                //TODO move url to some config
                BaseAddress = new Uri("http://localhost:8099/api/Products")
            };

            httpClient.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", userService.Token);

            client = RestClient.For<IProductsService>(httpClient);
        }

        public Task<List<ProductDto>> GetAll()
        {
            return client.GetAll();
        }

        public async Task<ProductDto> GetByCode([Path] string code)
        {
            return await client.GetByCode(code);
        }

        public async Task<CreateProductDraftResult> PostDraft([Body] CreateProductDraftCommand request)
        {
            return await client.PostDraft(request);
        }

        public async Task<ActivateProductResult> Activate([Body] ActivateProductCommand request)
        {
            return await client.Activate(request);
        }

        public async Task<DiscontinueProductResult> Discontinue([Body] DiscontinueProductCommand request)
        {
            return await client.Discontinue(request);
        }
    }
}
