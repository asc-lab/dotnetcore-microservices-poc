using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using MediatR;
using PaymentService.Api.Queries;

namespace PaymentService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IMediator bus;

        public PaymentController(IMediator bus)
        {
            this.bus = bus;
        }

        [HttpGet("accounts/{policyNumber}")]
        public async Task<ActionResult> AccountBalance(string policyNumber)
        {
            var result = await bus.Send(new GetAccountBalanceQuery {PolicyNumber = policyNumber});
            return new JsonResult(result);
        }
    }
}