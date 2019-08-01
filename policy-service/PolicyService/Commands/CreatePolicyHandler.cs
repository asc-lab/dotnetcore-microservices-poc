using MediatR;
using PolicyService.Api.Commands;
using PolicyService.Api.Events;
using PolicyService.Domain;
using PolicyService.Messaging;
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
        private readonly IEventPublisher eventPublisher;

        public CreatePolicyHandler(
            IUnitOfWorkProvider uowProvider,
            IEventPublisher eventPublisher)
        {
            this.uowProvider = uowProvider;
            this.eventPublisher = eventPublisher;
        }

        public async Task<CreatePolicyResult> Handle(CreatePolicyCommand request, CancellationToken cancellationToken)
        {
            using (var uow = uowProvider.Create())
            {
                var offer = await uow.Offers.WithNumber(request.OfferNumber);
                var customer = new PolicyHolder
                (
                    request.PolicyHolder.FirstName,
                    request.PolicyHolder.LastName,
                    request.PolicyHolder.TaxId,
                    Address.Of
                    (
                        request.PolicyHolderAddress.Country,
                        request.PolicyHolderAddress.ZipCode,
                        request.PolicyHolderAddress.City,
                        request.PolicyHolderAddress.Street
                    )
                );
                var policy = offer.Buy(customer);

                uow.Policies.Add(policy);

                await uow.CommitChanges();

                await eventPublisher.PublishMessage(PolicyCreated(policy));

                return new CreatePolicyResult
                {
                    PolicyNumber = policy.Number
                };
                
            }
        }

        private PolicyCreated PolicyCreated(Policy policy)
        {
            var version = policy.Versions.First(v => v.VersionNumber == 1);

            return new PolicyCreated
            {
                PolicyNumber = policy.Number,
                PolicyFrom = version.CoverPeriod.ValidFrom,
                PolicyTo = version.CoverPeriod.ValidTo,
                ProductCode = policy.ProductCode,
                TotalPremium = version.TotalPremiumAmount,
                PolicyHolder = new Api.Commands.Dtos.PersonDto
                {
                    FirstName = version.PolicyHolder.FirstName,
                    LastName = version.PolicyHolder.LastName,
                    TaxId = version.PolicyHolder.Pesel
                },
                AgentLogin = policy.AgentLogin
            };
        }
    }
}
