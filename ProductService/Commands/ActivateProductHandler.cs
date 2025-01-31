using MediatR;
using ProductService.Api.Commands;
using ProductService.Domain;

namespace ProductService.Commands;

public class ActivateProductHandler(IProductRepository products)
    : IRequestHandler<ActivateProductCommand, ActivateProductResult>
{
    public async Task<ActivateProductResult> Handle(ActivateProductCommand request, CancellationToken cancellationToken)
    {
        var product = await products.FindById(request.ProductId);
        product.Activate();
        return new ActivateProductResult
        {
            ProductId = product.Id
        };
    }
}