using MediatR;
using ProductService.Api.Queries.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProductService.Api.Queries
{
    public class FindAllProductsQuery : IRequest<IEnumerable<ProductDto>>
    {

    }
}
