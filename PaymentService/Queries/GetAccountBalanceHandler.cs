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
        private readonly IUnitOfWork unitOfWork;

        public GetAccountBalanceHandler(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<GetAccountBalanceQueryResult> Handle(GetAccountBalanceQuery request, CancellationToken cancellationToken)
        {
            var policyAccount = unitOfWork.PolicyAccounts.FindByNumber(request.PolicyNumber);
            
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