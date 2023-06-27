using System;
using System.Collections.Generic;
using DashboardService.Domain;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.Aggregations;
using Elastic.Clients.Elasticsearch.QueryDsl;
using Microsoft.VisualBasic;
using Steeltoe.Common.Util;

namespace DashboardService.DataAccess.Elastic;

public class SalesTrendsQueryAdapter : QueryAdapter<SalesTrendsQuery, SalesTrendsResult, PolicyDocument>
{
    private SalesTrendsQueryAdapter(SalesTrendsQuery query) : base(query)
    {
    }

    public static SalesTrendsQueryAdapter For(SalesTrendsQuery query)
    {
        return new SalesTrendsQueryAdapter(query);
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

        var histogram = new DateHistogramAggregation("sales")
        {
            Field = new Field("from"),
            Interval = new Union<DateInterval, Time>(query.AggregationUnit.ToDateInterval()),
            Aggregations = new SumAggregation("total_premium", new Field("totalPremium"))
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

    public override SalesTrendsResult ExtractResult(SearchResponse<PolicyDocument> searchResponse)
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