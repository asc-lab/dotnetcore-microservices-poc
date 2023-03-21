using System.Collections.Generic;
using DashboardService.Api.Queries.Dtos;

namespace DashboardService.Api.Queries;

public class GetSalesTrendsResult
{
    public List<PeriodSaleDto> PeriodsSales { get; set; }
}