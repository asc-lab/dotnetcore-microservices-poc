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

        /*
        var filteredAgg = new FilterAggregation("agg_filter")
        {
            Filter = filter,
            Aggregations = termAgg
        };*/

        return new SearchRequest<PolicyDocument>
        {
            Query = filter,
            Aggregations = new AggregationDictionary(new Dictionary<string, Aggregation> { ["agg_filter"] = termAgg })
        };
    }

    public override TotalSalesQueryResult ExtractResult(SearchResponse<PolicyDocument> searchResponse)
    {
        var result = new TotalSalesQueryResult();
/*
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
                new SalesResult(bucket.DocCount ?? 0, Convert.ToDecimal(sum.Value ?? 0.0))
            );
        }
*/

        var countByProduct = searchResponse
            .Aggregations
            .GetStringTerms("count_by_product");
            
            
        foreach (var bucket in countByProduct.Buckets)
        {
            decimal? sum = 0.00M; /*bucket.Sum("total_premium");*/
            result.ProductResult
            (
                bucket.Key,
                new SalesResult(bucket.DocCount ?? 0, Convert.ToDecimal(sum.Value ?? 0.0))
            );
        }
        
        return result;
    }
}