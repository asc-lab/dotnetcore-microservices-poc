using AgentPortalApiGateway.RestClients;
using Microsoft.Extensions.DependencyInjection;

namespace PolicyService.RestClients
{
    public static class RestClientsInstaller
    {
        public static IServiceCollection AddPricingRestClient(this IServiceCollection services)
        {
            services.AddSingleton(typeof(IProductClient), typeof(ProductClient));

            return services;
        }
    }
}
