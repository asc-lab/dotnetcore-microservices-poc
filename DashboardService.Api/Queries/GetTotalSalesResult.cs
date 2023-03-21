using System.Collections.Generic;
using DashboardService.Api.Queries.Dtos;

namespace DashboardService.Api.Queries;

public class GetTotalSalesResult
{
    public SalesDto Total { get; set; }

    public Dictionary<string, SalesDto> PerProductTotal { get; set; }
}