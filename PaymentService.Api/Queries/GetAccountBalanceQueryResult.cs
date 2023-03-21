using PaymentService.Api.Queries.Dtos;

namespace PaymentService.Api.Queries;

public class GetAccountBalanceQueryResult
{
    public PolicyAccountBalanceDto Balance { get; set; }
}