using DashboardService.Api.Queries;
using FluentAssertions;
using FluentAssertions.Primitives;

namespace DashboardService.Test;

public static class GetTotalSalesResultAssert
{
    public static GetTotalSalesResultAssertions Should(this GetTotalSalesResult subject)
    {
        return new GetTotalSalesResultAssertions(subject);
    }
}

public class
    GetTotalSalesResultAssertions : ReferenceTypeAssertions<GetTotalSalesResult, GetTotalSalesResultAssertions>
{
    public GetTotalSalesResultAssertions(GetTotalSalesResult subject) : base(subject)
    {
    }

    protected override string Identifier => "GetTotalSalesResultAssertions";

    public AndConstraint<GetTotalSalesResultAssertions> HaveTotal(long count, decimal premium)
    {
        Subject.Total.PoliciesCount.Should().Be(count);
        Subject.Total.PremiumAmount.Should().Be(premium);
        return new AndConstraint<GetTotalSalesResultAssertions>(this);
    }

    public AndConstraint<GetTotalSalesResultAssertions> HaveProductTotal(string product, long count,
        decimal premium)
    {
        var productTotal = Subject.PerProductTotal[product];
        productTotal.Should().NotBeNull();
        productTotal.PoliciesCount.Should().Be(count);
        productTotal.PremiumAmount.Should().Be(premium);
        return new AndConstraint<GetTotalSalesResultAssertions>(this);
    }
}