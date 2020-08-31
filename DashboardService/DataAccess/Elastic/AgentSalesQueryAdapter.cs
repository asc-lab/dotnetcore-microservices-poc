using System;
using System.Collections.Generic;
using DashboardService.Domain;
using Nest;

namespace DashboardService.DataAccess.Elastic
{
    public class AgentSalesQueryAdapter : QueryAdapter<AgentSalesQuery,AgentSalesQueryResult,PolicyDocument>
    {
        private AgentSalesQueryAdapter(AgentSalesQuery query) : base(query)
        {
        }
        
        public static AgentSalesQueryAdapter For(AgentSalesQuery agentSalesQuery) 
            => new AgentSalesQueryAdapter(agentSalesQuery);

        public override SearchRequest<PolicyDocument> BuildQuery()
        {
            var filters = new List<QueryContainer>();
            
            if (!string.IsNullOrWhiteSpace(query.FilterByAgentLogin))
            {
                filters.Add(new TermQuery
                {
                    Field = new Field("agentLogin.keyword"),
                    Value = query.FilterByAgentLogin
                });
            }
            
            if (!string.IsNullOrWhiteSpace(query.FilterByProductCode))
            {
                filters.Add(new TermQuery
                {
                    Field = new Field("productCode.keyword"),
                    Value = query.FilterByProductCode
                });
            }
            
            if (query.FilterBySalesDateStart != default || query.FilterBySalesDateEnd != default)
            {
                filters.Add(new DateRangeQuery
                {
                    Field = new Field("from"),
                    GreaterThanOrEqualTo = query.FilterBySalesDateStart,
                    LessThanOrEqualTo = query.FilterBySalesDateEnd
                });
            }
            
            if (filters.Count == 0)
            {
                filters.Add(new MatchAllQuery());
            }
            
            var filter = new BoolQuery
            {
                Must = filters
            };

            var sumAmountAgg = new SumAggregation("total_premium",new Field("totalPremium"));

            var termAgg = new TermsAggregation("count_by_agent")
            {
                Field = new Field("agentLogin.keyword"),
                Aggregations = sumAmountAgg
            };
            
            var filteredAgg = new FilterAggregation("agg_filter")
            {
                Filter = filter,
                Aggregations = termAgg
            };
            
            
            return new SearchRequest<PolicyDocument>
            {
                Aggregations = filteredAgg
            };
        }

        public override AgentSalesQueryResult ExtractResult(ISearchResponse<PolicyDocument> searchResponse)
        {
            var result = new AgentSalesQueryResult();

            var countByAgent = searchResponse
                .Aggregations
                .Nested("agg_filter")
                .Terms("count_by_agent");

            foreach (var bucket in countByAgent.Buckets)
            {
                var sum = bucket.Sum("total_premium");
                
                result.AgentResult
                (
                    bucket.Key, 
                    new SalesResult
                    (
                        bucket.DocCount ?? 0, 
                        Convert.ToDecimal(sum.Value ?? 0.0)
                    )
                );
            }
            
            return result;
        }
    }
}