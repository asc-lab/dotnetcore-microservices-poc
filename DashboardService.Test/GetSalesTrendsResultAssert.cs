using System.Linq;
using DashboardService.Api.Queries;
using FluentAssertions;
using FluentAssertions.Primitives;

namespace DashboardService.Test;

public static class GetSalesTrendsResultAssert
{
    public static GetSalesTrendsResultAssertions Should(this GetSalesTrendsResult result)
    {
        return new GetSalesTrendsResultAssertions(result);
    }
}

public class
    GetSalesTrendsResultAssertions : ReferenceTypeAssertions<GetSalesTrendsResult, GetSalesTrendsResultAssertions>
{
    public GetSalesTrendsResultAssertions(GetSalesTrendsResult subject) : base(subject)
    {
    }

    protected override string Identifier => "GetSalesTrendsResult";

    public AndConstraint<GetSalesTrendsResultAssertions> HavePeriods(int expectedNumberOfPeriods)
    {
        Subject.PeriodsSales.Count.Should().Be(expectedNumberOfPeriods);

        return new AndConstraint<GetSalesTrendsResultAssertions>(this);
    }

    public AndConstraint<GetSalesTrendsResultAssertions> HaveSalesForMonth(int year, int month,
        decimal expectedSales, int expectedCount)
    {
        var salesInMonth = Subject
            .PeriodsSales
            .Where(p => p.PeriodDate.Year == year && p.PeriodDate.Month == month)
            .Sum(p => p.Sales.PremiumAmount);
        var countInMonth = Subject
            .PeriodsSales
            .Where(p => p.PeriodDate.Year == year && p.PeriodDate.Month == month)
            .Sum(p => p.Sales.PoliciesCount);

        salesInMonth.Should().Be(expectedSales, $"we expected {expectedSales} for {year}-{month}");
        countInMonth.Should().Be(countInMonth, $"we expected {countInMonth} for {year}-{month}");

        return new AndConstraint<GetSalesTrendsResultAssertions>(this);
    }
}