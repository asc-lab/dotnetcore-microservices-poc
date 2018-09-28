using Microsoft.AspNetCore.Builder;
using System;
using Microsoft.Extensions.DependencyInjection;

namespace PricingService.Init
{
    public static class ApplicationBuilderExtensions
    {
        public static void UseInitializer(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var initializer = scope.ServiceProvider.GetService<Init.DataLoader>();
                initializer.Seed();
            }
        }
    }
}
