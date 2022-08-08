using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.DTO;
using WebApi.Services;

namespace WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private UsersService usersService;

        public UsersController(UsersService usersService)
        {
            this.usersService = usersService;
        }

        [HttpGet("all")]
        public async Task<List<UserInfoDto>> Get()
        {
            return await usersService.GetAll();
        }

        [HttpGet("{id}")]
        public async Task<UserInfoDto> Get(int id)
        {
            return await usersService.GetById(id);
        }

        [HttpPost("register")]
        public async Task<JsonResult> Register(UserRegisterDto user)
        {
            string token = await usersService.Register(user);
            return await Task.FromResult(new JsonResult(new
            {
                email = user.Email,
                accessToken = token
            }));
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login([FromBody] UserLoginDto user)
        {
            string token = await usersService.Login(user);
            if (token is null)
                return await Task.FromResult(Unauthorized());

            return await Task.FromResult(new JsonResult(new
            {
                email = user.Email,
                accessToken = token
            }));
        }
    }
}