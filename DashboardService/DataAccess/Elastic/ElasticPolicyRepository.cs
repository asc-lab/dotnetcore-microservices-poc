using System.Linq;
using DashboardService.Domain;
using Nest;

namespace DashboardService.DataAccess.Elastic
{
    public class ElasticPolicyRepository : IPolicyRepository
    {
        private readonly ElasticClient elasticClient;

        public ElasticPolicyRepository()
        {
            var connectionSettings = new ConnectionSettings()
                .DefaultMappingFor<PolicyDocument>(m=>m.IndexName("policy_lab_stats").IdProperty(d=>d.Number));
            elasticClient = new ElasticClient(connectionSettings);
        }

        public ElasticPolicyRepository(ElasticClient elasticClient)
        {
            this.elasticClient = elasticClient;
            
            //var connectionSettings = new ConnectionSettings();
            //connectionSettings.DefaultIndex("policies");
            //this.elasticClient = new ElasticClient(connectionSettings);

            //this.elasticClient.CreateIndex("policies", c => c.Mappings(ms => ms.Map<Policy>(m => m.AutoMap())));
        }

        public void Save(PolicyDocument policy)
        {
            elasticClient.Index
            (
                policy, 
                i => i
                    .Index("policy_lab_stats")
                    .Id(policy.Number)
            );
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
                                    new Field("number.keyword"),policyNumber)))
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
}