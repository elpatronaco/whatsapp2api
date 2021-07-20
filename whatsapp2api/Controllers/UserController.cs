using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using whatsapp2api.Contracts.Services;
using whatsapp2api.Models.User;

namespace whatsapp2api.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : Controller
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<UserModel>>> Get()
        {
            var users = await _service.GetAllUsers();

            return Ok(users);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<List<UserModel>>> Get(Guid id)
        {
            var user = await _service.GetUserById(id);

            if (user == null) return NotFound("No user matches this id");

            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<UserModel>> Post([FromBody] UserCreate userBody)
        {
            var isUserExist = await _service.DoesUserExist(userBody.Phone);

            if (isUserExist) return Conflict("User already exists");

            var user = await _service.CreateUser(userBody);

            return user != null ? Ok(user) : Problem("User could not be created");
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<UserModel>> Delete(Guid id)
        {
            var user = await _service.GetUserById(id);

            if (user == null) return NotFound("No user matches this id");

            await _service.DeleteUser(id);

            return Ok(user);
        }
    }
}