using Microsoft.AspNetCore.Mvc.Testing;
using ProductService.Api.Queries.Dtos;
using ProductService.Test.TestData;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using static Xunit.Assert;

namespace ProductService.Test.Controllers
{
    public class ProductsControllerTest : IClassFixture<WebApplicationFactory<Startup>>
    {
        private readonly WebApplicationFactory<Startup> factory;

        public ProductsControllerTest(WebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
        }

        [Fact]
        public async Task GetAll_ReturnsJsonResult_WithListOfProducts()
        {
            var client = factory.CreateClient();

            var response = await client.DoGetAsync<List<ProductDto>>("/api/Products");
            
            True(response.Count > 1);
        }


        [Fact]
        public async Task GetByCode_ReturnsJsonResult_WithOneProductOfCorrectType()
        {
            var productTravel = TestProductDtoFactory.Travel();

            var client = factory.CreateClient();

            var response = await client.DoGetAsync<ProductDto>("/api/Products/" + productTravel.Code);

            Equal(productTravel.Code, response.Code);
            Equal(productTravel.Name, response.Name);
            Equal(productTravel.Description, response.Description);
        }


    }
}
