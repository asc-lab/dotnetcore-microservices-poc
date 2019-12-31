using System.Collections.Generic;
using DashboardService.Api.Queries.Dtos;

namespace DashboardService.Api.Queries
{
    public class GetAgentsSalesResult
    {
        public Dictionary<string, SalesDto> PerAgentTotal { get; set; }
    }
}