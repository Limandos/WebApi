using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using WebApi.DTO;
using WebApi.Services;

namespace WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize(Roles = "user, admin")]
    public class OrdersController : ControllerBase
    {
        private OrdersService ordersService;

        public OrdersController(OrdersService ordersService)
        {
            this.ordersService = ordersService;
        }

        [HttpGet("all")]
        public async Task<List<OrderDto>> GetAll()
        {
            return await ordersService.GetAll();
        }

        [HttpPost("add")]
        public async Task<OrderDto> AddOrder([FromBody] List listProductsDto)
        {
            return await ordersService.MakeOrder(User.FindFirstValue(ClaimTypes.Email), listProductsDto.Serials);
        }

        [HttpDelete("delete/{id}")]
        public async Task<OrderDto> Delete(int id)
        {
            return await ordersService.DeleteOrder(id);
        }
    }
}