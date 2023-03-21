using System.Collections.Generic;
using PolicySearchService.Api.Queries.Dtos;

namespace PolicySearchService.Api.Queries;

public class FindPolicyResult
{
    public List<PolicyDto> Policies { get; set; }
}