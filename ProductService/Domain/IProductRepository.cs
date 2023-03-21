namespace ProductService.Domain;

public interface IProductRepository
{
    Task<Product> Add(Product product);

    Task<List<Product>> FindAllActive();

    Task<Product> FindOne(string productCode);

    Task<Product> FindById(Guid id);
}