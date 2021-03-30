using AgentPortalUi.BlazorWasm.Contracts.Dto;
using RestEase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AgentPortalUi.BlazorWasm.Contracts
{
    public interface IProductsService
    {
        // GET api/products
        [Get]
        Task<List<ProductDto>> GetAll();

        // GET api/products/{code}
        [Get("{code}")]
        Task<ProductDto> GetByCode([Path] string code);

        // POST api/products
        [Post]
        Task<CreateProductDraftResult> PostDraft([Body] CreateProductDraftCommand request);

        // POST api/products/activate
        [Post("/activate")]
        Task<ActivateProductResult> Activate([Body] ActivateProductCommand request);

        // POST api/products/discontinue
        [Post("/discontinue")]
        Task<DiscontinueProductResult> Discontinue([Body] DiscontinueProductCommand request);
    }
}
