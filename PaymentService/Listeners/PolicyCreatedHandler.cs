using MediatR;
using PaymentService.Domain;
using PolicyService.Api.Events;
using System.Threading;
using System.Threading.Tasks;

namespace PaymentService.Listeners
{
    public class PolicyCreatedHandler : INotificationHandler<PolicyCreated>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly PolicyAccountNumberGenerator policyAccountNumberGenerator;

        public PolicyCreatedHandler(IUnitOfWork unitOfWork, PolicyAccountNumberGenerator policyAccountNumberGenerator)
        {
            this.unitOfWork = unitOfWork;
            this.policyAccountNumberGenerator = policyAccountNumberGenerator;
        }

        public async Task Handle(PolicyCreated notification, CancellationToken cancellationToken)
        {
            var policy = new PolicyAccount(notification.PolicyNumber, policyAccountNumberGenerator.Generate());

            using (unitOfWork)
            {
                unitOfWork.PolicyAccounts.Add(policy);
                unitOfWork.CommitChanges();
            }
        }
    }
}
