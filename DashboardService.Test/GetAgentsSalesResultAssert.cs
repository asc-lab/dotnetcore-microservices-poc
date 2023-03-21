using DashboardService.Api.Queries;
using FluentAssertions;
using FluentAssertions.Primitives;

namespace DashboardService.Test;

public static class GetAgentsSalesResultAssert
{
    public static GetAgentsSalesResultAssertions Should(this GetAgentsSalesResult result)
    {
        return new GetAgentsSalesResultAssertions(result);
    }
}

public class
    GetAgentsSalesResultAssertions : ReferenceTypeAssertions<GetAgentsSalesResult, GetAgentsSalesResultAssertions>
{
    public GetAgentsSalesResultAssertions(GetAgentsSalesResult subject) : base(subject)
    {
    }

    protected override string Identifier => "GetAgentsSalesResult";

    public AndConstraint<GetAgentsSalesResultAssertions> HaveAgentSales(string agent, long count, decimal premium)
    {
        var agentSales = Subject.PerAgentTotal[agent];
        agentSales.Should().NotBeNull();
        agentSales.PoliciesCount.Should().Be(count);
        agentSales.PremiumAmount.Should().Be(premium);

        return new AndConstraint<GetAgentsSalesResultAssertions>(this);
    }

    public AndConstraint<GetAgentsSalesResultAssertions> HaveResultsForAgents(int expectedAgentsCount)
    {
        Subject.PerAgentTotal.Count.Should().Be(expectedAgentsCount);
        return new AndConstraint<GetAgentsSalesResultAssertions>(this);
    }
}