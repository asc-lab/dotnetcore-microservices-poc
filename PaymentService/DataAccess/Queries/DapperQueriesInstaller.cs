using Microsoft.Extensions.DependencyInjection;
using PaymentService.Queries;

namespace PaymentService.DataAccess.Queries
{
    public static class DapperQueriesInstaller
    {
        public static void AddDapperQueries(this IServiceCollection services, string cnnString)
        {
            services.AddSingleton<IPolicyAccountQueries>(new PolicyAccountQueries(cnnString));
        }
    }
}
