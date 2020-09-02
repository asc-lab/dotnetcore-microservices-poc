using System;
using System.Collections.Generic;
using DashboardService.Domain;
using Nest;

namespace DashboardService.DataAccess.Elastic
{
    public class TotalSalesQueryAdapter : QueryAdapter<TotalSalesQuery,TotalSalesQueryResult,PolicyDocument>
    {
        private TotalSalesQueryAdapter(TotalSalesQuery query) : base(query)
        {
        }
        
        public static TotalSalesQueryAdapter For(TotalSalesQuery totalSalesQuery) 
            => new TotalSalesQueryAdapter(totalSalesQuery);

        public override SearchRequest<PolicyDocument> BuildQuery()
        {
            var filters = new List<QueryContainer>();
            
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

            
            var termAgg = new TermsAggregation("count_by_product")
            {
                Field = new Field("productCode.keyword"),
                Aggregations = new SumAggregation("total_premium",new Field("totalPremium"))
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

        public override TotalSalesQueryResult ExtractResult(ISearchResponse<PolicyDocument> searchResponse)
        {
            var result = new TotalSalesQueryResult();

            var countByProduct = searchResponse
                .Aggregations
                .Nested("agg_filter")
                .Terms("count_by_product");

            foreach (var bucket in countByProduct.Buckets)
            {
                var sum = bucket.Sum("total_premium");
                result.ProductResult
                (
                bucket.Key, 
                new SalesResult(bucket.DocCount ?? 0,Convert.ToDecimal(sum.Value ?? 0.0))
                );
            }
            
            return result;
        }
    }
}