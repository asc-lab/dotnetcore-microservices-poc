using Microsoft.Extensions.DependencyInjection;
using Nest;
using PolicySearchService.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PolicySearchService.DataAccess.ElasticSearch
{
    public static class NestInstaller
    {
        public static IServiceCollection AddElasticSearch(this IServiceCollection services, string cnString)
        {
            services.AddSingleton(typeof(ElasticClient), svc => CreateElasticClient(cnString));
            services.AddScoped(typeof(IPolicyRepository), typeof(PolicyRepository));
            return services;
        }

        private static ElasticClient CreateElasticClient(string cnString)
        {
            var settings = new ConnectionSettings(new Uri(cnString))
                .DefaultIndex("lab_policies");
            var client = new ElasticClient(settings);
            return client;
        }
    }
}
