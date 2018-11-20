using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using ProductService.Api.Queries;
using ProductService.Api.Queries.Dtos;
using ProductService.Controllers;
using ProductService.Test.TestData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace ProductService.Test.Controllers
{
    public class ProductsControllerTest
    {      
        [Fact]
        public async Task GetAll_ReturnsJsonResult_WithListofProducts()
        {            
            var mockMediator = new Mock<IMediator>();
            mockMediator.Setup(x => x.Send(It.IsAny<FindAllProductsQuery>(), It.IsAny<CancellationToken>())).ReturnsAsync(new List<ProductDto> { TestProductDtoFactory.Travel() } );

            var controller = new ProductsController(mockMediator.Object);

            var result = await controller.GetAll();

            var jsonResult = Assert.IsType<JsonResult>(result);
            var items = Assert.IsType<List<ProductDto>>(jsonResult.Value);
            Assert.Single(items);            
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
