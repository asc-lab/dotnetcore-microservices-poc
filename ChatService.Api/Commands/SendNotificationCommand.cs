using MediatR;

namespace ChatService.Api.Commands;

public class SendNotificationCommand : IRequest<Unit>
{
    public string Message { get; set; }
}