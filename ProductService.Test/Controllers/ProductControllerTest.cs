using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using ProductService.Api.Queries;
using ProductService.Api.Queries.Dtos;
using ProductService.Controllers;
using ProductService.Test.TestData;
using System.Collections.Generic;
using System.Threading;
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
        public async Task GetAll_ReturnsJsonResult_WithListofProducts()
        {
            var client = factory.CreateClient();

            var response = await client.DoGetAsync<List<ProductDto>>("/api/Product");

            True(response.Count > 1);
        }


        [Fact]
        public async Task GetByCode_ReturnsJsonResult_WithOneProductOfCorrectType()
        {
            var paramProductCode = TestProductDtoFactory.Travel().Code;

            var mockMediator = new Mock<IMediator>();
            mockMediator.Setup(x => x.Send(It.IsAny<FindProductByCodeQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(TestProductDtoFactory.Travel());

            var controller = new ProductsController(mockMediator.Object);

            var result = await controller.GetByCode(paramProductCode);

            var jsonResult = Assert.IsType<JsonResult>(result);
            var item = Assert.IsType<ProductDto>(jsonResult.Value);
            Assert.Equal(paramProductCode, item.Code);
        }


    }
}
