using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductService.Api.Queries;

namespace ProductService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IMediator mediator;

        public ProductsController(IMediator mediator)
        {
            this.mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        // GET api/products
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var result = await mediator.Send(new FindAllProductsQuery());
            return new JsonResult(result);
        }

        // GET api/products/{code}
        [HttpGet("{code}")]
        public async Task<ActionResult> GetByCode([FromRoute]string code)
        {
            var result = await mediator.Send(new FindProductByCodeQuery{ ProductCode = code });                        
            return new JsonResult(result);
        }
        
    }    
}
