using MediatR;
using ProductService.Api.Commands.Dtos;

namespace ProductService.Api.Commands;

public class CreateProductDraftCommand : IRequest<CreateProductDraftResult>
{
    public ProductDraftDto ProductDraft { get; set; }
}