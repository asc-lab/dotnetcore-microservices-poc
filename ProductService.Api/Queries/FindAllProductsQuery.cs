using System.Collections.Generic;
using MediatR;
using ProductService.Api.Queries.Dtos;

namespace ProductService.Api.Queries
{
    public class FindAllProductsQuery : IRequest<IEnumerable<ProductDto>>
    {

    }
}
