using System;
using System.Threading.Tasks;
using DashboardService.DataAccess.Elastic;
using DashboardService.Domain;
using Xunit;

namespace DashboardService.Test
{
    [Collection("ElasticSearch in a container")]
    public class GetAgentsSalesQueryTest 
    {
        private ElasticSearchInContainerFixture fixture;

        public GetAgentsSalesQueryTest(ElasticSearchInContainerFixture fixture)
        {
            this.fixture = fixture;
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
        
        [Fact]
        public async Task SavedPolicy_CanBeFoundWith_FindByNumber2()
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