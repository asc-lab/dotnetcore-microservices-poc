using System.Linq;

namespace ProductService.DataAccess.EF.Data
{
    public class DbInitializer
    {
        public static void Initialize(ProductDbContext context)
        {
            context.Database.EnsureCreated();
            if (context.Products.Any())
            {
                return;
            }

            context.Products.Add(DemoProductFactory.Travel());
            context.Products.Add(DemoProductFactory.House());
            context.Products.Add(DemoProductFactory.Farm());
            context.Products.Add(DemoProductFactory.Car());

            context.SaveChanges();
        }
    }
}
