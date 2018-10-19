using MediatR;
using PolicyService.Api.Commands;
using PolicyService.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PolicyService.Commands
{
    public class CreatePolicyHandler : IRequestHandler<CreatePolicyCommand, CreatePolicyResult>
    {
        private readonly IUnitOfWorkProvider uowProvider;

        public CreatePolicyHandler(IUnitOfWorkProvider uowProvider)
        {
            this.uowProvider = uowProvider;
        }

        public async Task<CreatePolicyResult> Handle(CreatePolicyCommand request, CancellationToken cancellationToken)
        {
            using (var uow = uowProvider.Create())
            {
                var offer = uow.Offers.WithNumber(request.OfferNumber);
                var customer = new PolicyHolder(
                    request.PolicyHolder.FirstName,
                    request.PolicyHolder.LastName,
                    request.PolicyHolder.TaxId);
                var policy = offer.Buy(customer);

                uow.Policies.Add(policy);

                uow.CommitChanges();

                return new CreatePolicyResult
                {
                    PolicyNumber = policy.Number
                };
                
            }
        }
    }
}
