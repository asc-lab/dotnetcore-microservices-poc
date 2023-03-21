using System.Threading.Tasks;
using ChatService.Api.Commands;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ChatService.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class NotificationController : ControllerBase
{
    private readonly IMediator bus;

    public NotificationController(IMediator bus)
    {
        this.bus = bus;
    }

    [HttpPost]
    public async Task<IActionResult> SendNotification([FromBody] SendNotificationCommand cmd)
    {
        var result = await bus.Send(cmd);
        return Ok();
    }
}