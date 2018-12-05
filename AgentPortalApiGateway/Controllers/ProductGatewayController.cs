using AgentPortalApiGateway.RestClients;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AgentPortalApiGateway.Controllers
{
    [Route("api/products")]
    [ApiController]
    public class ProductGatewayController : ControllerBase
    {
        private readonly IProductClient client;

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var result = await client.GetAll();
            return new JsonResult(result);
        }

        [HttpGet("{code}")]
        public async Task<ActionResult> GetByCode([FromRoute] string code)
        {
            var result = await client.GetByCode(code);
            return new JsonResult(result);
        }
    }
}
