using ProductService.DataAccess.EF;

namespace ProductService.Init;

public class DataLoader(ProductDbContext context)
{
    public void Seed()
    {
        context.Database.EnsureCreated();
        if (context.Products.Any()) return;

        context.Products.Add(DemoProductFactory.Travel());
        context.Products.Add(DemoProductFactory.House());
        context.Products.Add(DemoProductFactory.Farm());
        context.Products.Add(DemoProductFactory.Car());

        context.SaveChanges();
    }
}