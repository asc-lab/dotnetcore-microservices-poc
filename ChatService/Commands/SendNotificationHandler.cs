using System.Threading;
using System.Threading.Tasks;
using ChatService.Api.Commands;
using ChatService.Hubs;
using MediatR;
using Microsoft.AspNetCore.SignalR;

namespace ChatService.Commands;

public class SendNotificationHandler : IRequestHandler<SendNotificationCommand, Unit>
{
    private readonly IHubContext<AgentChatHub> chatHubContext;

    public SendNotificationHandler(IHubContext<AgentChatHub> chatHubContext)
    {
        this.chatHubContext = chatHubContext;
    }

    public async Task<Unit> Handle(SendNotificationCommand request, CancellationToken cancellationToken)
    {
        await chatHubContext.Clients.All.SendAsync("ReceiveMessage", "system", request.Message);

        return Unit.Value;
    }
}