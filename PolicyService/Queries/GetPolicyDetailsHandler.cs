using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PolicyService.Api.Queries;
using PolicyService.Api.Queries.Dto;
using PolicyService.Domain;

namespace PolicyService.Queries;

public class GetPolicyDetailsHandler : IRequestHandler<GetPolicyDetailsQuery, GetPolicyDetailsQueryResult>
{
    private readonly IUnitOfWork uow;

    public GetPolicyDetailsHandler(IUnitOfWork uow)
    {
        this.uow = uow;
    }

    public async Task<GetPolicyDetailsQueryResult> Handle(GetPolicyDetailsQuery request,
        CancellationToken cancellationToken)
    {
        var policy = await uow.Policies.WithNumber(request.PolicyNumber);
        if (policy == null) throw new ApplicationException($"Policy {request.PolicyNumber} not found!");

        return ConstructResult(policy);
    }

    private GetPolicyDetailsQueryResult ConstructResult(Policy policy)
    {
        var effectiveVersion = policy.Versions.FirstVersion();

        return new GetPolicyDetailsQueryResult
        {
            Policy = new PolicyDetailsDto
            {
                Number = policy.Number,
                ProductCode = policy.ProductCode,
                DateFrom = effectiveVersion.CoverPeriod.ValidFrom,
                DateTo = effectiveVersion.CoverPeriod.ValidTo,
                PolicyHolder = $"{effectiveVersion.PolicyHolder.FirstName} {effectiveVersion.PolicyHolder.LastName}",
                TotalPremium = effectiveVersion.TotalPremiumAmount,

                AccountNumber = null,
                Covers = effectiveVersion.Covers.Select(c => c.Code).ToList()
            }
        };
    }
}