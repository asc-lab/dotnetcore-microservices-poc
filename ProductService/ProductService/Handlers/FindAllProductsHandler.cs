using MediatR;
using Microsoft.EntityFrameworkCore;
using ProductService.Api.Queries;
using ProductService.Api.Queries.Dtos;
using ProductService.DataAccess.EF;
using ProductService.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ProductService.Queries
{
    public class FindAllProductsHandler : IRequestHandler<FindAllProductsQuery, IEnumerable<ProductDto>>
    {
        private readonly ProductDbContext productDbContext;        

        public FindAllProductsHandler(ProductDbContext dbContext)
        {
            productDbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));            
        }

        public async Task<IEnumerable<ProductDto>> Handle(FindAllProductsQuery request, CancellationToken cancellationToken)
        {
           var result = productDbContext.Products.Include(c => c.Covers).Include("Questions.Choices").ToList();

            return result.Select(p => new ProductDto
            {
                Code = p.Code,
                Name = p.Name,
                Description = p.Description,
                Image = p.Image,
                MaxNumberOfInsured = p.MaxNumberOfInsured,
                Questions = p.Questions != null ? ProductMapper.ToQuestionDtoList(p.Questions) : null,
                Covers = p.Covers.Any() ? ProductMapper.ToCoverDtoList(p.Covers) : null
            }).ToList();
        }      
        
    }
}
