using Microsoft.AspNetCore.Mvc;
using PaymentService.Api.Exceptions;
using PaymentService.Queries;
using System.Threading.Tasks;

namespace PaymentService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPolicyAccountQueries policyAccountQueries;

        public PaymentController(IPolicyAccountQueries policyAccountQueries)
        {
            this.policyAccountQueries = policyAccountQueries;
        }

        [HttpGet]
        public async Task<ActionResult> Accounts()
        {
            var result = await policyAccountQueries.FindAll();
            return new JsonResult(result);
        }

        [HttpGet("accounts/{accountNumber}")]
        public async Task<ActionResult> AccountBalance(string accountNumber)
        {
            var result = await policyAccountQueries.FindByNumber(accountNumber);
            if (result == null) {
                throw new PolicyAccountNotFound(accountNumber);
            }
            return new JsonResult(result);
        }
    }
}