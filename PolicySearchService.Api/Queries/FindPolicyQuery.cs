using MediatR;

namespace PolicySearchService.Api.Queries;

public class FindPolicyQuery : IRequest<FindPolicyResult>
{
    public string QueryText { get; set; }
}