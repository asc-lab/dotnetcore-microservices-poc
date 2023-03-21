using MediatR;

namespace PolicyService.Api.Queries;

public class GetPolicyDetailsQuery : IRequest<GetPolicyDetailsQueryResult>
{
    public string PolicyNumber { get; set; }
}