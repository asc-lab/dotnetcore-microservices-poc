﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;
using ProductService.Domain;
using ProductService.Queries;
using ProductService.Test.TestData;
using Xunit;

namespace ProductService.Test.Handlers
{
    public class FindProductsHandlersTest
    {
        private readonly Mock<IProductRepository> productRepository;

        private readonly List<Product> products = new List<Product>
        {
            TestProductFactory.Travel(),
            TestProductFactory.House()
        };

        public FindProductsHandlersTest()
        {
            productRepository = new Mock<IProductRepository>();


            productRepository.Setup(x => x.FindAllActive()).Returns(Task.FromResult(products));
            productRepository.Setup(x => x.FindOne(It.Is<string>(s => products.Select(p => p.Code).Contains(s)))).Returns(Task.FromResult(products.First()));
            productRepository.Setup(x => x.FindOne(It.Is<string>(s => !products.Select(p => p.Code).Contains(s)))).Returns(Task.FromResult<Product>(null));
        }

        [Fact]
        public async Task FindAllProductsHandler_ReturnsListOfAllProducts()
        {
            var findAllProductsHandler = new FindAllProductsHandler(productRepository.Object);

            var result = await findAllProductsHandler.Handle(new Api.Queries.FindAllProductsQuery(), new System.Threading.CancellationToken());

            Assert.NotNull(result);
            Assert.Equal(products.Count, result.Count());
        }

        [Fact]
        public async Task FindProductByCodeHandler_ReturnsOneProduct()
        {
            var findProductByCodeHandler = new FindProductByCodeHandler(productRepository.Object);

            var result = await findProductByCodeHandler.Handle(new Api.Queries.FindProductByCodeQuery { ProductCode = TestProductFactory.Travel().Code }, new System.Threading.CancellationToken());

            Assert.NotNull(result);
        }

        [Fact]
        public async Task FindProductByCodeHandler_ReturnsNullIfCodeNotExists()
        {
            var findProductByCodeHandler = new FindProductByCodeHandler(productRepository.Object);

            var result = await findProductByCodeHandler.Handle(new Api.Queries.FindProductByCodeQuery { ProductCode = "ASDASD" }, new System.Threading.CancellationToken());

            Assert.Null(result);
        }
    }
}
