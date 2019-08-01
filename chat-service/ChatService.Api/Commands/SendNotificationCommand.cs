using MediatR;

namespace ChatService.Api.Commands
{
    public class SendNotificationCommand : IRequest
    {
        public string Message { get; set; }
    }
}