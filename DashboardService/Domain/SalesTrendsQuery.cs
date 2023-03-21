using System;
using System.Collections.Generic;
using Nest;
using TimeUnit = DashboardService.Api.Queries.Dtos.TimeUnit;

namespace DashboardService.Domain;

public class SalesTrendsQuery
{
    public SalesTrendsQuery(string filterByProductCode, DateTime filterBySalesDateStart, DateTime filterBySalesDateEnd,
        TimeAggregationUnit aggregationUnit)
    {
        FilterByProductCode = filterByProductCode;
        FilterBySalesDateStart = filterBySalesDateStart;
        FilterBySalesDateEnd = filterBySalesDateEnd;
        AggregationUnit = aggregationUnit;
    }

    public string FilterByProductCode { get; }
    public DateTime FilterBySalesDateStart { get; }
    public DateTime FilterBySalesDateEnd { get; }
    public TimeAggregationUnit AggregationUnit { get; }
}

public enum TimeAggregationUnit
{
    Day,
    Week,
    Month,
    Year
}

public static class TimeAggregationUnitExtensions
{
    public static DateInterval ToDateInterval(this TimeAggregationUnit unit)
    {
        return unit switch
        {
            TimeAggregationUnit.Day => DateInterval.Day,
            TimeAggregationUnit.Week => DateInterval.Week,
            TimeAggregationUnit.Month => DateInterval.Month,
            TimeAggregationUnit.Year => DateInterval.Year,
            _ => throw new ArgumentException($"Invalid value of unit {unit}")
        };
    }

    public static TimeAggregationUnit ToTimeAggregationUnit(this TimeUnit unit)
    {
        return unit switch
        {
            TimeUnit.Day => TimeAggregationUnit.Day,
            TimeUnit.Week => TimeAggregationUnit.Week,
            TimeUnit.Month => TimeAggregationUnit.Month,
            TimeUnit.Year => TimeAggregationUnit.Year,
            _ => throw new ArgumentException($"Invalid value of unit {unit}")
        };
    }
}

public class SalesTrendsResult
{
    public SalesTrendsResult()
    {
        PeriodSales = new List<PeriodSales>();
    }

    public IList<PeriodSales> PeriodSales { get; }

    public void PeriodResult(PeriodSales periodSales)
    {
        PeriodSales.Add(periodSales);
    }
}

public class PeriodSales
{
    public PeriodSales(DateTime periodDate, string period, SalesResult sales)
    {
        PeriodDate = periodDate;
        Period = period;
        Sales = sales;
    }

    public DateTime PeriodDate { get; }
    public string Period { get; }
    public SalesResult Sales { get; }
}