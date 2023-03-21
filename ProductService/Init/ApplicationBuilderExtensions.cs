namespace ProductService.Init;

public static class ApplicationBuilderExtensions
{
    public static void UseInitializer(this IApplicationBuilder app)
    {
        using (var scope = app.ApplicationServices.CreateScope())
        {
            var initializer = scope.ServiceProvider.GetService<DataLoader>();
            initializer.Seed();
        }
    }
}