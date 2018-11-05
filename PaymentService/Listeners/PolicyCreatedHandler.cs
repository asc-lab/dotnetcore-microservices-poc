using MediatR;
using PaymentService.Domain;
using PolicyService.Api.Events;
using System.Threading;
using System.Threading.Tasks;

namespace PaymentService.Listeners
{
    public class PolicyCreatedHandler : INotificationHandler<PolicyCreated>
    {
        private readonly IUnitOfWorkProvider uowProvider;
        private readonly PolicyAccountNumberGenerator policyAccountNumberGenerator;

        public PolicyCreatedHandler(IUnitOfWorkProvider uowProvider, PolicyAccountNumberGenerator policyAccountNumberGenerator)
        {
            this.uowProvider = uowProvider;
            this.policyAccountNumberGenerator = policyAccountNumberGenerator;
        }

        public async Task Handle(PolicyCreated notification, CancellationToken cancellationToken)
        {
            var policy = new PolicyAccount(notification.PolicyNumber, policyAccountNumberGenerator.Generate());

            using (var uow = uowProvider.Create())
            {
                uow.PolicyAccountRespository.Add(policy);
                uow.CommitChanges();
            }
        }
    }
}
