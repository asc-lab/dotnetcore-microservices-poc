using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PolicyService.Api.Commands;
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

                policy.Terminate(request.TerminationDate);
                
                await uow.CommitChanges();

                return new TerminatePolicyResult
                {
                    PolicyNumber = policy.Number,
                    MoneyToReturn = 0M
                };
            }
        }
    }
}