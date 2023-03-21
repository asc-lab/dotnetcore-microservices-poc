using System.Threading;
using System.Threading.Tasks;
using ChatService.Hubs;
using MediatR;
using Microsoft.AspNetCore.SignalR;
using PolicyService.Api.Events;

namespace ChatService.Listeners;

public class PolicyCreatedHandler : INotificationHandler<PolicyCreated>
{
    private readonly IHubContext<AgentChatHub> chatHubContext;

    public PolicyCreatedHandler(IHubContext<AgentChatHub> chatHubContext)
    {
        this.chatHubContext = chatHubContext;
    }

    public async Task Handle(PolicyCreated notification, CancellationToken cancellationToken)
    {
        await chatHubContext.Clients.All.SendAsync("ReceiveNotification",
            $"{notification.AgentLogin} just sold policy for {notification.ProductCode}!!!");
    }
}