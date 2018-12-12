using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProductService.Domain
{
    public interface IProductRepository
    {
        Task<Product> Add(Product product);

        Task<List<Product>> FindAllActive();

        Task<Product> FindOne(String productCode);
        
        Task<Product> FindById(Guid id);
    }
}
