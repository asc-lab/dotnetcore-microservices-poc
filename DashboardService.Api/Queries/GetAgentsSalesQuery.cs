using System;
using MediatR;

namespace DashboardService.Api.Queries;

public class GetAgentsSalesQuery : IRequest<GetAgentsSalesResult>
{
    public string AgentLogin { get; set; }
    public string ProductCode { get; set; }
    public DateTime SalesDateFrom { get; set; }
    public DateTime SalesDateTo { get; set; }
}