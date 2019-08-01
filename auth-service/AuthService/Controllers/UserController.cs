using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthService.Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly Domain.AuthService authService;

        public UserController(Domain.AuthService authService)
        {
            this.authService = authService;
        }

        [AllowAnonymous]
        [HttpPost]
        public IActionResult Post([FromBody] AuthRequest user)
        {
            var token = authService.Authenticate(user.Login, user.Password);

            if (token == null)
                return BadRequest(new {message = "Username of password incorrect"});

            return Ok(new {Token = token});
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(authService.AgentFromLogin(HttpContext.User.Identity.Name));
        }

    }
}