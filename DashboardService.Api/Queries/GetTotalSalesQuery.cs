using System;
using MediatR;

namespace DashboardService.Api.Queries;

public class GetTotalSalesQuery : IRequest<GetTotalSalesResult>
{
    public string ProductCode { get; set; }
    public DateTime SalesDateFrom { get; set; }
    public DateTime SalesDateTo { get; set; }
}