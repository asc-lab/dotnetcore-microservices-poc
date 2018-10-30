using MediatR;
using PolicySearchService.Api.Queries;
using PolicySearchService.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PolicySearchService.Queries
{
    public class FindPolicyHandler : IRequestHandler<FindPolicyQuery, FindPolicyResult>
    {
        private readonly IPolicyRepository policis;

        public FindPolicyHandler(IPolicyRepository policis)
        {
            this.policis = policis;
        }

        public Task<FindPolicyResult> Handle(FindPolicyQuery request, CancellationToken cancellationToken)
        {
            var searchResults = policis.Find(request.QueryText);

            return FindPolicyResult(searchResults);
        }

        private Task<FindPolicyResult> FindPolicyResult(List<Policy> searchResults)
        {
            var result = new FindPolicyResult
            {
                Policies = searchResults.Select(p => new Api.Queries.Dtos.PolicyDto
                {
                    PolicyNumber = p.PolicyNumber,
                    PolicyStartDate = p.PolicyStartDate,
                    PolicyEndDate = p.PolicyEndDate,
                    ProductCode = p.ProductCode,
                    PolicyHolder = p.PolicyHolder,
                    PremiumAmount = p.PremiumAmount
                })
                .ToList()
            };
            return Task.FromResult(result);
        }
    }
}
