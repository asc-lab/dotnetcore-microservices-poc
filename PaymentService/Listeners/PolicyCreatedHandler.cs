using MediatR;
using PolicyService.Api.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PaymentService.Listeners
{
    public class PolicyCreatedHandler : INotificationHandler<PolicyCreated>
    {
        public async Task Handle(PolicyCreated notification, CancellationToken cancellationToken)
        {
            Console.WriteLine("msg received");
        }
    }
}
