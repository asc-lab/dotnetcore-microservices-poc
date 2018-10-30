using Nest;
using PolicySearchService.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PolicySearchService.DataAccess.ElasticSearch
{
    public class PolicyRepository : IPolicyRepository
    {
        private readonly ElasticClient elasticClient;

        public PolicyRepository(ElasticClient elasticClient)
        {
            this.elasticClient = elasticClient;
        }

        public void Add(Policy policy)
        {
            elasticClient.IndexDocument(policy);
        }

        public List<Policy> Find(string queryText)
        {
            return elasticClient
                .Search<Policy>(
                    s =>
                        s.From(0)
                        .Size(10)
                        .Query(q =>
                            q.MultiMatch(mm =>
                                mm.Query(queryText)
                                .Fields(f => f.Fields(p => p.PolicyNumber, p => p.PolicyHolder))
                                .Type(TextQueryType.BestFields)
                                .Fuzziness(Fuzziness.Auto)
                            )
                    ))
                    .Documents.ToList();
        }
    }
}
