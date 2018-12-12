using System;
using MediatR;

namespace ProductService.Api.Commands
{
    public class ActivateProductCommand : IRequest<ActivateProductResult>
    {
        public Guid ProductId { get; set; }
    }
}