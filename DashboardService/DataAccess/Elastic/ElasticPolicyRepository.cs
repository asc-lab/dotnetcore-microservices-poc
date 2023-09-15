using System;
using System.Linq;
using DashboardService.Domain;
using Elastic.Clients.Elasticsearch;

namespace DashboardService.DataAccess.Elastic;

public class ElasticPolicyRepository : IPolicyRepository
{
    private readonly ElasticsearchClient elasticClient;

    public ElasticPolicyRepository(ElasticsearchClient elasticClient)
    {
        this.elasticClient = elasticClient;
    }

    public void Save(PolicyDocument policy)
    {
        var response = elasticClient.Index
        (
            policy,
            i => i
                .Index("policy_lab_stats")
                .Id(policy.Number)
                .Refresh(Refresh.True)
        );

        if (!response.IsValidResponse) throw new ApplicationException("Failed to index a policy document");
    }

    public PolicyDocument FindByNumber(string policyNumber)
    {
        var searchResponse = elasticClient.Search<PolicyDocument>
        (
            s => s
                .Query(q => q
                    .Bool(b => b
                        .Filter(bf => bf
                            .Term(
                                 p => new Field("number.keyword"), policyNumber)))
                )
        );

        return searchResponse.Documents.FirstOrDefault();
    }

    public AgentSalesQueryResult GetAgentSales(AgentSalesQuery query)
    {
        var adapter = AgentSalesQueryAdapter.For(query);
        var response = elasticClient.Search<PolicyDocument>(adapter.BuildQuery());
        return adapter.ExtractResult(response);
    }

    public TotalSalesQueryResult GetTotalSales(TotalSalesQuery query)
    {
        var adapter = TotalSalesQueryAdapter.For(query);
        var response = elasticClient.Search<PolicyDocument>(adapter.BuildQuery());
        return adapter.ExtractResult(response);
    }

    public SalesTrendsResult GetSalesTrend(SalesTrendsQuery query)
    {
        var adapter = SalesTrendsQueryAdapter.For(query);
        var response = elasticClient.Search<PolicyDocument>(adapter.BuildQuery());
        return adapter.ExtractResult(response);
    }
}