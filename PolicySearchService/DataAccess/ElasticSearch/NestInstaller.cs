using Microsoft.Extensions.DependencyInjection;
using Nest;
using PolicySearchService.Domain;
using System;

namespace PolicySearchService.DataAccess.ElasticSearch
{
    public static class NestInstaller
    {
        public static IServiceCollection AddElasticSearch(this IServiceCollection services, ElasticSearchOptions options)
        {
            services.AddSingleton(typeof(ElasticClient), svc => CreateElasticClient(options.Connection));
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
