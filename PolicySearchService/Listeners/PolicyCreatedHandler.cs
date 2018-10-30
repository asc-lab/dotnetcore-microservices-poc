using MediatR;
using PolicySearchService.Domain;
using PolicyService.Api.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PolicySearchService.Listeners
{
    public class PolicyCreatedHandler : INotificationHandler<PolicyCreated>
    {
        private readonly IPolicyRepository policis;

        public PolicyCreatedHandler(IPolicyRepository policis)
        {
            this.policis = policis;
        }

        public Task Handle(PolicyCreated notification, CancellationToken cancellationToken)
        {
            policis.Add(new Policy
            {
                PolicyNumber = notification.PolicyNumber,
                PolicyStartDate = notification.PolicyFrom,
                PolicyEndDate = notification.PolicyTo,
                ProductCode = notification.ProductCode,
                PolicyHolder = $"{notification.PolicyHolder.FirstName} {notification.PolicyHolder.LastName}",
                PremiumAmount = notification.TotalPremium,
            });

            return Task.CompletedTask;
        }
    }
}
