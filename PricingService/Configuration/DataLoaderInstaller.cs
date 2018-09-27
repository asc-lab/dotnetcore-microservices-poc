using Microsoft.Extensions.DependencyInjection;
using PricingService.Init;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PricingService.Configuration
{
    public static class DataLoaderInstaller
    {
        public static IServiceCollection AddPricingDemoInitializer(this IServiceCollection services)
        {
            services.AddSingleton<DataLoader>();
            return services;
        }
    }
}
