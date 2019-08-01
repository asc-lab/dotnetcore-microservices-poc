using System;
using MediatR;

namespace ProductService.Api.Commands
{
    public class DiscontinueProductCommand : IRequest<DiscontinueProductResult>
    {
        public Guid ProductId { get; set; }
    }
}