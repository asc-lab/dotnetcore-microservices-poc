using System;
using DashboardService.Domain;
using Microsoft.Extensions.DependencyInjection;
using Nest;

namespace DashboardService.DataAccess.Elastic
{
    public static class NestInstaller
    {
        public static IServiceCollection AddElasticSearch(this IServiceCollection services, string cnString)
        {
            services.AddSingleton(typeof(ElasticClient), svc => CreateElasticClient(cnString));
            services.AddScoped(typeof(IPolicyRepository), typeof(ElasticPolicyRepository));
            return services;
        }

        private static ElasticClient CreateElasticClient(string cnString)
        {
            var connectionSettings = new ConnectionSettings(new Uri(cnString))
                .DefaultMappingFor<PolicyDocument>(m=>
                    m.IndexName("policy_lab_stats").IdProperty(d=>d.Number));
            return new ElasticClient(connectionSettings);
        }
    }
}
