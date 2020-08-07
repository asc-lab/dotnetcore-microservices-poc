using System.Linq;
using DashboardService.Api.Queries;
using FluentAssertions;
using FluentAssertions.Primitives;
using Xunit;

namespace DashboardService.Test
{
    public static class GetSalesTrendsResultAssert
    {
        public static GetSalesTrendsResultAssertions Should(this GetSalesTrendsResult result) => new GetSalesTrendsResultAssertions(result);
    }

    public class GetSalesTrendsResultAssertions : ReferenceTypeAssertions<GetSalesTrendsResult, GetSalesTrendsResultAssertions>
    {
        protected override string Identifier => "GetSalesTrendsResult";

        public GetSalesTrendsResultAssertions(GetSalesTrendsResult subject) : base(subject)
        {
        }

        public AndConstraint<GetSalesTrendsResultAssertions> HavePeriods(int expectedNumberOfPeriods)
        {
            Assert.Equal(expectedNumberOfPeriods, Subject.PeriodsSales.Count);

            return new AndConstraint<GetSalesTrendsResultAssertions>(this);
        }
        
        public AndConstraint<GetSalesTrendsResultAssertions> HaveSalesForMonth(int year, int month, decimal expectedSales, int expectedCount)
        {
            var salesInMonth = Subject
                .PeriodsSales
                .Where(p => p.PeriodDate.Year == year && p.PeriodDate.Month == month)
                .Sum(p => p.Sales.PremiumAmount);
            var countInMonth = Subject
                .PeriodsSales
                .Where(p => p.PeriodDate.Year == year && p.PeriodDate.Month == month)
                .Sum(p => p.Sales.PoliciesCount);
            Assert.Equal(expectedSales, salesInMonth);
            Assert.Equal(expectedCount, countInMonth);

            
            return new AndConstraint<GetSalesTrendsResultAssertions>(this);
        }
    }
}