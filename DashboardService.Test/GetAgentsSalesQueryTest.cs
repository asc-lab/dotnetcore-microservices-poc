using System;
using System.Threading;
using DashboardService.Api.Queries;
using DashboardService.DataAccess.Elastic;
using DashboardService.Queries;
using Xunit;

namespace DashboardService.Test;

[Collection("ElasticSearch in a container")]
public class GetAgentsSalesQueryTest
{
    private readonly ElasticSearchInContainerFixture fixture;

    public GetAgentsSalesQueryTest(ElasticSearchInContainerFixture fixture)
    {
        this.fixture = fixture;
    }

    [Fact]
    public async void Sales_All_Product_By_Jim_In_01_And_02_2020()
    {
        var queryHandler = new GetAgentsSalesHandler(new ElasticPolicyRepository(fixture.ElasticClient()));
        var result = await queryHandler.Handle(
            new GetAgentsSalesQuery
            {
                AgentLogin = "jimmy.solid",
                SalesDateFrom = new DateTime(2020, 1, 1),
                SalesDateTo = new DateTime(2020, 2, 28)
            },
            CancellationToken.None);

        result.Should()
            .HaveResultsForAgents(1)
            .And
            .HaveAgentSales("jimmy.solid", 6, 630M);
    }

    [Fact]
    public async void Sales_All_Product_By_All_Agents_In_01_And_02_2020()
    {
        var queryHandler = new GetAgentsSalesHandler(new ElasticPolicyRepository(fixture.ElasticClient()));
        var result = await queryHandler.Handle(
            new GetAgentsSalesQuery
            {
                SalesDateFrom = new DateTime(2020, 1, 1),
                SalesDateTo = new DateTime(2020, 2, 28)
            },
            CancellationToken.None);

        result.Should()
            .HaveResultsForAgents(2)
            .And
            .HaveAgentSales("jimmy.solid", 6, 630M)
            .And
            .HaveAgentSales("danny.solid", 5, 325M);
    }
}