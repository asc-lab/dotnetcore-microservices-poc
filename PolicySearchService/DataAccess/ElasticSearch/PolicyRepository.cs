using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Elastic.Clients.Elasticsearch;
using Elastic.Clients.Elasticsearch.QueryDsl;
using PolicySearchService.Domain;

namespace PolicySearchService.DataAccess.ElasticSearch;

public class PolicyRepository : IPolicyRepository
{
    private readonly ElasticsearchClient elasticClient;

    public PolicyRepository(ElasticsearchClient elasticClient)
    {
        this.elasticClient = elasticClient;
    }

    public async Task Add(Policy policy)
    {
        await elasticClient.IndexAsync(policy);
    }

    public async Task<List<Policy>> Find(string queryText)
    {
        var result = await elasticClient
            .SearchAsync<Policy>(
                s =>
                    s.From(0)
                        .Size(10)
                        .Query(q =>
                            q.MultiMatch(mm =>
                                mm.Query(queryText)
                                    .Fields(Infer.Fields<Policy>(p=>p.PolicyNumber, p=>p.PolicyHolder))
                                    .Type(TextQueryType.BestFields)
                                    .Fuzziness(new Fuzziness("AUTO"))
                            )
                        ));

        return result.Documents.ToList();
    }
}