using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using ProductService.Api.Queries.Dtos;
using ProductService.Test.TestData;
using Xunit;
using static Xunit.Assert;

namespace ProductService.Test.Controllers;

public class ProductsControllerTest(WebApplicationFactory<Program> factory)
    : IClassFixture<WebApplicationFactory<Program>>
{
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