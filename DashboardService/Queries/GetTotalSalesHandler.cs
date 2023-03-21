using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using DashboardService.Api.Queries;
using DashboardService.Api.Queries.Dtos;
using DashboardService.Domain;
using MediatR;

namespace DashboardService.Queries;

public class GetTotalSalesHandler : IRequestHandler<GetTotalSalesQuery, GetTotalSalesResult>
{
    private readonly IPolicyRepository policyRepository;

    public GetTotalSalesHandler(IPolicyRepository policyRepository)
    {
        this.policyRepository = policyRepository;
    }

    public Task<GetTotalSalesResult> Handle(GetTotalSalesQuery request, CancellationToken cancellationToken)
    {
        var queryResult = policyRepository.GetTotalSales
        (
            new TotalSalesQuery
            (
                request.ProductCode,
                request.SalesDateFrom,
                request.SalesDateTo
            )
        );

        return Task.FromResult(BuildResult(queryResult));
    }

    private GetTotalSalesResult BuildResult(TotalSalesQueryResult queryResult)
    {
        var result = new GetTotalSalesResult
        {
            Total = new SalesDto(queryResult.Total.PoliciesCount, queryResult.Total.PremiumAmount),
            PerProductTotal = new Dictionary<string, SalesDto>()
        };

        foreach (var productTotal in queryResult.PerProductTotal)
            result.PerProductTotal[productTotal.Key] = new SalesDto
            (
                productTotal.Value.PoliciesCount,
                productTotal.Value.PremiumAmount
            );

        return result;
    }
}