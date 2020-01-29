using System;
using System.Threading.Tasks;
using DashboardService.DataAccess.Elastic;
using DashboardService.Domain;
using DotNet.Testcontainers.Containers.Builders;
using DotNet.Testcontainers.Containers.Modules;
using DotNet.Testcontainers.Containers.WaitStrategies;
using Nest;
using Xunit;

namespace DashboardService.Test
{
    public class ElasticPolicyRepositoryTest
    {
        [Fact]
        public async Task SavedPolicy_CanBeFoundWith_FindByNumber()
        {
            //docker run -d --name elasticsearch -p 9200:9200 -p 9300:9300 elasticsearch:7.5.1	

            var testContainersBuilder = new TestcontainersBuilder<TestcontainersContainer>()
                .WithImage("elasticsearch:6.4.0")
                .WithName("elasticsearch-33333")
                .WithEnvironment("discovery.type","single-node")
                .WithPortBinding(9200, 9200)
                .WithPortBinding(9300, 9300)
                //.WithCleanUp(true)        
            //.WithWaitStrategy(Wait.UntilContainerIsRunning());    
            .WithWaitStrategy(Wait.UntilPortsAreAvailable(9200));

            using var testContainer = testContainersBuilder.Build();
            await testContainer.StartAsync();

            var policyRepo = new ElasticPolicyRepository(CreateElasticClient());
            
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
            
            /*
             * Elasticsearch.Net.UnexpectedElasticsearchClientException: Cannot deserialize the current JSON object (e.g. {"name":"value"}) into type 'System.Int64' because the type requires a JSON primitive value (e.g. string, number, boolean, null) to deserialize correctly.
To fix this error either change the JSON to a JSON primitive value (e.g. string, number, boolean, null) or change the deserialized type so that it is a normal .NET type (e.g. not a primitive type like integer, not a collection type like an array or List<T>) that can be deserialized from a JSON object. JsonObjectAttribute can also be added to the type to force it to deserialize from a JSON object.
Path 'hits.total.value', line 1, position 114.
 ---> Nest.Json.JsonSerializationException: Cannot deserialize the current JSON object (e.g. {"name":"value"}) into type 'System.Int64' because the type requires a JSON primitive value (e.g. string, number, boolean, null) to deserialize correctly.
To fix this error either change the JSON to a JSON primitive value (e.g. string, number, boolean, null) or change the deserialized type so that it is a normal .NET type (e.g. not a primitive type like integer, not a collection type like an array or List<T>) that can be deserialized from a JSON object. JsonObjectAttribute can also be added to the type to force it to deserialize from a JSON object.
Path 'hits.total.value', line 1, position 114.
   at Nest.Json.Serialization
             */

        }
        
        private ElasticClient CreateElasticClient()
        {
            var connectionSettings = new ConnectionSettings()
                .DefaultMappingFor<PolicyDocument>(m=>
                    m.IndexName("policy_lab_stats").IdProperty(d=>d.Number));
            return new ElasticClient(connectionSettings);
        }
    }
}