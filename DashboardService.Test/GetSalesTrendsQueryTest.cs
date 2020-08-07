using System;
using System.Threading;
using System.Threading.Tasks;
using DashboardService.Api.Queries;
using DashboardService.Api.Queries.Dtos;
using DashboardService.DataAccess.Elastic;
using DashboardService.Domain;
using DashboardService.Queries;
using Xunit;

namespace DashboardService.Test
{
    [Collection("ElasticSearch in a container")]
    public class GetSalesTrendsQueryTest
    {
        private ElasticSearchInContainerFixture fixture;

        public GetSalesTrendsQueryTest(ElasticSearchInContainerFixture fixture)
        {
            this.fixture = fixture;
        }

        [Fact]
        public async Task Sales_All_Product_In_First_Q_2020()
        {
            var queryHandler = new GetSalesTrendsHandler(new ElasticPolicyRepository(fixture.ElasticClient()));

            var result = await queryHandler.Handle(new GetSalesTrendsQuery
                {
                    Unit    = TimeUnit.Month,
                    SalesDateFrom = new DateTime(2020,1,1),
                    SalesDateTo = new DateTime(2020,3,31)
                }, 
                CancellationToken.None);
            
            result
                .Should()
                .HavePeriods(3)
                .And
                .HaveSalesForMonth(2020,1,450M,4)
                .And
                .HaveSalesForMonth(2020,2,180,2)
                .And
                .HaveSalesForMonth(2020,3,690,6)
                ;
        }
        
        [Fact]
        public async Task SavedPolicy_CanBeFoundWith_FindByNumber()
        {
            var policyRepo = new ElasticPolicyRepository(fixture.ElasticClient());
            
            var pol = new PolicyDocument
            (
                "POL1201",
                new DateTime(2019,1,1),
                new DateTime(2019,12,31),
                "Jan Ziomalski",
                "BDA",
                4500M,
                "jim beam"
            );
            
            policyRepo.Save(pol);

            var saved = policyRepo.FindByNumber("POL1201");
            
            Assert.NotNull(saved);
        }
    }
}