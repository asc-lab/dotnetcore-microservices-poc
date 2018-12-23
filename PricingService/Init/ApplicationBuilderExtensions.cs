using Microsoft.AspNetCore.Builder;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace PricingService.Init
{
    public static class ApplicationBuilderExtensions
    {
        public static async Task UseInitializer(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var initializer = scope.ServiceProvider.GetService<Init.DataLoader>();
                await initializer.Seed();
            }
        }
    }
}
