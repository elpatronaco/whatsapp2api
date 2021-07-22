using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using whatsapp2api.Contracts.Services;
using whatsapp2api.Models.Auth;
using whatsapp2api.Models.Chat;
using whatsapp2api.Models.Message;
using whatsapp2api.Models.User;
using whatsapp2api.Services;

namespace whatsapp2api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : Controller
    {
        private readonly IUserService _service;
        private readonly IHubContext<ChatHub> _hub;

        public AuthController(IUserService service, IHubContext<ChatHub> hub)
        {
            _service = service;
            _hub = hub;
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

        [HttpGet("test")]
        public async Task<ActionResult<string>> Test()
        {
            var user = new UserModel() {Id = Guid.NewGuid(), Phone = "615927034", Username = "elPatron"};
            var chat = new OpenChat()
            {
                Recipient = user,
                LastMessage =
                    new MessageModel()
                    {
                        Sender = user, Recipient = user, Content = "hola", Id = Guid.NewGuid(), SentDate = DateTime.Now
                    }
            };

            await _hub.Clients.All.SendCoreAsync("chats", new object?[] {chat, chat});

            return "hola";
        }
    }
}