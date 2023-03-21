using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PolicySearchService.Api.Queries;
using PolicySearchService.Api.Queries.Dtos;
using PolicySearchService.Domain;

namespace PolicySearchService.Queries;

public class FindPolicyHandler : IRequestHandler<FindPolicyQuery, FindPolicyResult>
{
    private readonly IPolicyRepository policis;

    public FindPolicyHandler(IPolicyRepository policis)
    {
        this.policis = policis;
    }

    public async Task<FindPolicyResult> Handle(FindPolicyQuery request, CancellationToken cancellationToken)
    {
        var searchResults = await policis.Find(request.QueryText);

        return FindPolicyResult(searchResults);
    }

    private FindPolicyResult FindPolicyResult(List<Policy> searchResults)
    {
        return new FindPolicyResult
        {
            Policies = searchResults.Select(p => new PolicyDto
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
    }
}