using System;
using Elastic.Clients.Elasticsearch;
using Microsoft.Extensions.DependencyInjection;
using PolicySearchService.Domain;

namespace PolicySearchService.DataAccess.ElasticSearch;

public static class NestInstaller
{
    public static IServiceCollection AddElasticSearch(this IServiceCollection services, string cnString)
    {
        services.AddSingleton(typeof(ElasticsearchClient), svc => CreateElasticClient(cnString));
        services.AddScoped(typeof(IPolicyRepository), typeof(PolicyRepository));
        return services;
    }

    private static ElasticsearchClient CreateElasticClient(string cnString)
    {
        var settings = new ElasticsearchClientSettings(new Uri(cnString))
            .DefaultIndex("lab_policies");
        var client = new ElasticsearchClient(settings);
        return client;
    }
}