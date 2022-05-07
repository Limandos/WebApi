using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
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

        [HttpGet]
        public ActionResult<IEnumerable<UserDto>> Get()
        {
            return new ActionResult<IEnumerable<UserDto>>(usersService.GetAll());
        }

        [HttpGet("{id}")]
        public ActionResult<UserDto> Get(int id)
        {
            return new ActionResult<UserDto>(usersService.Get(id));
        }

        [HttpPost]
        public ActionResult<UserDto> Post(UserDto user)
        {
            return new ActionResult<UserDto>(usersService.Post(user));
        }
    }
}
