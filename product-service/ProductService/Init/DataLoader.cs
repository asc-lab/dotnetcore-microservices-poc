using ProductService.DataAccess.EF;
using System.Linq;

namespace ProductService.Init
{
    public class DataLoader
    {

        private ProductDbContext _dbContext;

        public DataLoader(ProductDbContext context)
        {
            _dbContext = context;
        }

        public void Seed()
        {
            _dbContext.Database.EnsureCreated();
            if (_dbContext.Products.Any())
            {
                return;
            }

            _dbContext.Products.Add(DemoProductFactory.Travel());
            _dbContext.Products.Add(DemoProductFactory.House());
            _dbContext.Products.Add(DemoProductFactory.Farm());
            _dbContext.Products.Add(DemoProductFactory.Car());

            _dbContext.SaveChanges();
        }
    }
}
