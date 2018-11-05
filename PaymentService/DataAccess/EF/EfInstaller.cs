using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PaymentService.Domain;
using PaymentService.Queries;

namespace PaymentService.DataAccess.EF
{
    public static class EfInstaller
    {
        public static void AddEf(this IServiceCollection services, string cnnString)
        {
            services.AddSingleton<IPolicyAccountQueries>(new PolicyAccountQueries(cnnString));
            services.AddEntityFrameworkSqlServer()
                    .AddDbContext<PaymentContext>(options =>
                    {
                        options.UseSqlServer(cnnString);
                    },
                    ServiceLifetime.Scoped);
            
            services.AddScoped<IUnitOfWorkProvider, UnitOfWorkProvider>();
        }
    }
}
