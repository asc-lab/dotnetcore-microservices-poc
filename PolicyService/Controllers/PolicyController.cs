using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using PolicyService.Api.Commands;

namespace PolicyService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PolicyController : ControllerBase
    {
        private readonly IMediator bus;

        public PolicyController(IMediator bus)
        {
            this.bus = bus;
        }

        // POST api/values
        [HttpPost]
        public async Task<ActionResult> Post([FromBody] CreatePolicyCommand cmd)
        {
            var result = await bus.Send(cmd);
            return new JsonResult(result);
        }
    }
}