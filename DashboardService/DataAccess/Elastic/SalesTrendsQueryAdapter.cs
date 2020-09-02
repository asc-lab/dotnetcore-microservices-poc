using System;
using System.Collections.Generic;
using DashboardService.Domain;
using Nest;

namespace DashboardService.DataAccess.Elastic
{
    public class SalesTrendsQueryAdapter : QueryAdapter<SalesTrendsQuery, SalesTrendsResult, PolicyDocument>
    {
        private SalesTrendsQueryAdapter(SalesTrendsQuery query) : base(query)
        {
        }
        
        public static SalesTrendsQueryAdapter For(SalesTrendsQuery query) => new SalesTrendsQueryAdapter(query);

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
            
            var histogram = new DateHistogramAggregation("sales")
            {
                Field = new Field("from"),
                Interval = new Union<DateInterval, Time>(query.AggregationUnit.ToDateInterval()),
                Aggregations = new SumAggregation("total_premium",new Field("totalPremium"))
            };

            var filteredAgg = new FilterAggregation("agg_filter")
            {
                Filter = filter,
                Aggregations = histogram
            };
            
            return new SearchRequest<PolicyDocument>
            {
                Aggregations = filteredAgg
            };
        }

        public override SalesTrendsResult ExtractResult(ISearchResponse<PolicyDocument> searchResponse)
        {
            var result = new SalesTrendsResult();

            var histogram = searchResponse
                .Aggregations
                .Nested("agg_filter")
                .DateHistogram("sales");

            foreach (var bucket in histogram.Buckets)
            {
                var periodStart = bucket.Date;
                var totalPremium = Convert.ToDecimal(bucket.Sum("total_premium").Value ?? 0.0);
                
                result.PeriodResult
                (
                    new PeriodSales
                    (
                        periodStart, 
                        periodStart.ToShortDateString(), 
                        new SalesResult
                        (
                            bucket.DocCount ?? 0,
                            totalPremium
                        )
                    )
                );
            }
            
            return result;
        }
    }
}