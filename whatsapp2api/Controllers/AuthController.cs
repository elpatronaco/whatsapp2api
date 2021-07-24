using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using whatsapp2api.Contracts.Services;
using whatsapp2api.Models.Auth;
using whatsapp2api.Models.User;

namespace whatsapp2api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : Controller
    {
        private readonly IUserService _service;

        public AuthController(IUserService service)
        {
            _service = service;
        }

        [HttpPost("login")]
        [ProducesResponseType(200)]
        [ProducesResponseType(401)]
        public async Task<ActionResult<Tokens>> Login([FromBody] UserAuthenticate owner)
        {
            var tokens = await _service.Authenticate(owner);

            if (tokens is null) return Unauthorized("Phone or password incorrect");

            Response.Cookies.Append("refresh", tokens.Item2,
                new CookieOptions
                {
                    HttpOnly = true, Secure = true, Expires = DateTimeOffset.Now.AddDays(7), SameSite = SameSiteMode.Lax
                });

            return Ok(tokens.Item1);
        }

        [HttpPost("register")]
        [ProducesResponseType(200)]
        [ProducesResponseType(409)]
        public async Task<ActionResult<Tokens>> Register([FromBody] UserCreate owner)
        {
            var tokens = await _service.Register(owner);

            if (tokens is null) return Conflict("User already exists");

            Response.Cookies.Append("refresh", tokens.Item2,
                new CookieOptions
                {
                    HttpOnly = true, Secure = true, Expires = DateTimeOffset.Now.AddDays(7), SameSite = SameSiteMode.Lax
                });

            return Ok(tokens.Item1);
        }
    }
}