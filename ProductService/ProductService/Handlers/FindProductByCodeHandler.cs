using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductService.Api.Queries;
using ProductService.Api.Queries.Dtos;
using ProductService.DataAccess.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProductService.Queries
{
    public class FindProductByCodeHandler : IRequestHandler<FindProductByCodeQuery, ProductDto>
    {
        private readonly ProductDbContext productDbContext;

        public FindProductByCodeHandler(ProductDbContext dbContext)
        {
            productDbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<ProductDto> Handle(FindProductByCodeQuery request, CancellationToken cancellationToken)
        {
            var result = productDbContext.Products.Include(c => c.Covers).Include("Questions.Choices").FirstOrDefault(p => p.Code.Equals(request.ProductCode, StringComparison.InvariantCultureIgnoreCase));

            return result != null ? new ProductDto
            {
                Code = result.Code,
                Name = result.Name,
                Description = result.Description,
                Image = result.Image,
                MaxNumberOfInsured = result.MaxNumberOfInsured,
                Questions = result.Questions != null ? ProductMapper.ToQuestionDtoList(result.Questions) : null,
                Covers = result.Covers != null ? ProductMapper.ToCoverDtoList(result.Covers) : null
            } : null;
        }
    }
}
