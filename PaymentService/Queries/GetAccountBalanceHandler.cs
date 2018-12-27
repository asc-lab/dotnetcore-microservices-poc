using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PaymentService.Api.Exceptions;
using PaymentService.Api.Queries;
using PaymentService.Api.Queries.Dtos;
using PaymentService.Domain;

namespace PaymentService.Queries
{
    public class GetAccountBalanceHandler : IRequestHandler<GetAccountBalanceQuery, GetAccountBalanceQueryResult>
    {
        private readonly IDataStore dataStore;

        public GetAccountBalanceHandler(IDataStore dataStore)
        {
            this.dataStore = dataStore;
        }

        public async Task<GetAccountBalanceQueryResult> Handle(GetAccountBalanceQuery request, CancellationToken cancellationToken)
        {
            var policyAccount = await dataStore.PolicyAccounts.FindByNumber(request.PolicyNumber);
            
            if (policyAccount == null)
            {
                throw new PolicyAccountNotFound(request.PolicyNumber);
            }

            return new GetAccountBalanceQueryResult
            {
                Balance = new PolicyAccountBalanceDto
                {
                    PolicyNumber = policyAccount.PolicyNumber,
                    PolicyAccountNumber = policyAccount.PolicyAccountNumber,
                    Balance = policyAccount.BalanceAt(DateTimeOffset.Now)
                }
            };
        }
    }
}