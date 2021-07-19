using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using whatsapp2api.Contracts;
using whatsapp2api.Models.User;

namespace whatsapp2api.Controllers
{
    [ApiController]
    [Route("api/users")]
    public class UserController : Controller
    {
        private readonly IUserRepository _repo;

        public UserController(IUserRepository repo)
        {
            _repo = repo;
        }

        [HttpGet]
        public async Task<ActionResult<List<UserModel>>> Get()
        {
            var users = await _repo.GetAllUsers();

            return Ok(users);
        }

        [HttpGet("{id:Guid}")]
        public async Task<ActionResult<List<UserModel>>> Get(Guid id)
        {
            var user = await _repo.GetUserById(id);

            if (user == null) return NotFound("No user matches this id");

            return Ok(user);
        }

        [HttpPost]
        public async Task<ActionResult<UserModel>> Post([FromBody] UserCreate userBody)
        {
            var isUserExist = await _repo.DoesUserExist(userBody.Phone);
            
            if (isUserExist)return Conflict("User already exists");
            
            var user = await _repo.CreateUser(userBody);

            return Ok(user);
        }
    }
}