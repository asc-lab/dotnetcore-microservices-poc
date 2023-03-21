using System;
using DashboardService.Api.Queries.Dtos;
using MediatR;

namespace DashboardService.Api.Queries;

public class GetSalesTrendsQuery : IRequest<GetSalesTrendsResult>
{
    public string ProductCode { get; set; }
    public DateTime SalesDateFrom { get; set; }
    public DateTime SalesDateTo { get; set; }
    public TimeUnit Unit { get; set; }
}