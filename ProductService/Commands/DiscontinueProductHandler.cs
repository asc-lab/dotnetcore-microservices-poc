using MediatR;
using ProductService.Api.Commands;
using ProductService.Domain;

namespace ProductService.Commands;

public class DiscontinueProductHandler(IProductRepository products)
    : IRequestHandler<DiscontinueProductCommand, DiscontinueProductResult>
{
    public async Task<DiscontinueProductResult> Handle
    (
        DiscontinueProductCommand request,
        CancellationToken cancellationToken
    )
    {
        var product = await products.FindById(request.ProductId);
        product.Discontinue();
        return new DiscontinueProductResult
        {
            ProductId = product.Id
        };
    }
}