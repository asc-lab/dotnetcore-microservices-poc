using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PolicyService.Api.Commands;
using PolicyService.Api.Events;
using PolicyService.Domain;
using PolicyService.Messaging;

namespace PolicyService.Commands
{
    public class TerminatePolicyHandler : IRequestHandler<TerminatePolicyCommand, TerminatePolicyResult>
    {
        private readonly IUnitOfWorkProvider uowProvider;
        private readonly IEventPublisher eventPublisher;

        public TerminatePolicyHandler(IUnitOfWorkProvider uowProvider, IEventPublisher eventPublisher)
        {
            this.uowProvider = uowProvider;
            this.eventPublisher = eventPublisher;
        }

        public async Task<TerminatePolicyResult> Handle(TerminatePolicyCommand request, CancellationToken cancellationToken)
        {
            using (var uow = uowProvider.Create())
            {
                var policy = await uow.Policies.WithNumber(request.PolicyNumber);

                var terminationResult = policy.Terminate(request.TerminationDate);
                
                await uow.CommitChanges();

                await eventPublisher.PublishMessage(PolicyTerminated(terminationResult));

                return new TerminatePolicyResult
                {
                    PolicyNumber = policy.Number,
                    MoneyToReturn = terminationResult.AmountToReturn
                };
            }
        }

        private PolicyTerminated PolicyTerminated(PolicyTerminationResult terminationResult)
        {
            return new PolicyTerminated
            {
                PolicyNumber = terminationResult.TerminalVersion.Policy.Number,
                PolicyFrom = terminationResult.TerminalVersion.CoverPeriod.ValidFrom,
                PolicyTo = terminationResult.TerminalVersion.CoverPeriod.ValidTo,
                ProductCode = terminationResult.TerminalVersion.Policy.ProductCode,
                TotalPremium = terminationResult.TerminalVersion.TotalPremiumAmount,
                AmountToReturn = terminationResult.AmountToReturn,
                PolicyHolder = new Api.Commands.Dtos.PersonDto
                {
                    FirstName = terminationResult.TerminalVersion.PolicyHolder.FirstName,
                    LastName = terminationResult.TerminalVersion.PolicyHolder.LastName,
                    TaxId = terminationResult.TerminalVersion.PolicyHolder.Pesel
                }
            };
        }
    }
}