using MediatR;

namespace PaymentService.Api.Queries;

public class GetAccountBalanceQuery : IRequest<GetAccountBalanceQueryResult>
{
    public string PolicyNumber { get; set; }
}