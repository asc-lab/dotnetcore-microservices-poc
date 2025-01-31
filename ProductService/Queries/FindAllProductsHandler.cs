using MediatR;
using ProductService.Api.Queries;
using ProductService.Api.Queries.Dtos;
using ProductService.Domain;

namespace ProductService.Queries;

public class FindAllProductsHandler(IProductRepository productRepository)
    : IRequestHandler<FindAllProductsQuery, IEnumerable<ProductDto>>
{
    
    public async Task<IEnumerable<ProductDto>> Handle(FindAllProductsQuery request, CancellationToken cancellationToken)
    {
        var result = await productRepository.FindAllActive();

        return result.Select(p => new ProductDto
        {
            Code = p.Code,
            Name = p.Name,
            Description = p.Description,
            Image = p.Image,
            MaxNumberOfInsured = p.MaxNumberOfInsured,
            Icon = p.ProductIcon,
            Questions = p.Questions != null ? ProductMapper.ToQuestionDtoList(p.Questions) : null,
            Covers = p.Covers.Any() ? ProductMapper.ToCoverDtoList(p.Covers) : null
        }).ToList();
    }
}