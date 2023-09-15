using System;
using System.Collections.Generic;
using DashboardService.Domain;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Aggregations;
using Elastic.Clients.Elasticsearch.QueryDsl;

namespace DashboardService.DataAccess.Elastic;

public class TotalSalesQueryAdapter : QueryAdapter<TotalSalesQuery, TotalSalesQueryResult, PolicyDocument>
{
    private TotalSalesQueryAdapter(TotalSalesQuery query) : base(query)
    {
    }

    public static TotalSalesQueryAdapter For(TotalSalesQuery totalSalesQuery)
    {
        return new TotalSalesQueryAdapter(totalSalesQuery);
    }

    public override SearchRequest<PolicyDocument> BuildQuery()
    {
        var filters = new List<Query>();

        if (!string.IsNullOrWhiteSpace(query.FilterByProductCode))
        {
            var tq = new TermQuery(new Field("productCode.keyword"))
            {
                Value = query.FilterByProductCode
            };
            filters.Add(tq);
        }


        if (query.FilterBySalesDateStart != default || query.FilterBySalesDateEnd != default)
        {
            var drq = new DateRangeQuery(new Field("from"))
            {
                Gte = query.FilterBySalesDateStart,
                Lte = query.FilterBySalesDateEnd
            };
            filters.Add(drq);
        }


        if (filters.Count == 0) filters.Add(new MatchAllQuery());

        var filter = new BoolQuery
        {
            Must = filters
        };


        var termAgg = new TermsAggregation("count_by_product")
        {
            Field = new Field("productCode.keyword"),
            Aggregations = new SumAggregation("total_premium", new Field("totalPremium"))
        };

        return new SearchRequest<PolicyDocument>
        {
            Query = filter,
            Aggregations = new AggregationDictionary(new Dictionary<string, Aggregation> { ["count_by_product"] = termAgg })
        };
    }

    public override TotalSalesQueryResult ExtractResult(SearchResponse<PolicyDocument> searchResponse)
    {
        var result = new TotalSalesQueryResult();

        var countByProduct = searchResponse
            .Aggregations
            .GetStringTerms("count_by_product");
            
            
        foreach (var bucket in countByProduct.Buckets)
        {
            var sum = (bucket["total_premium"] as SumAggregate).Value ?? 0; 
            result.ProductResult
            (
                bucket.Key.ToString(),
                new SalesResult(bucket.DocCount, Convert.ToDecimal(sum))
            );
        }
        
        return result;
    }
}