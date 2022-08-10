using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.DTO;
using WebApi.Exceptions;
using WebApi.Services;

namespace WebApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BrandsController : ControllerBase
    {
        private BrandsService brandsService;

        public BrandsController(BrandsService brandsService)
        {
            this.brandsService = brandsService;
        }

        [HttpGet("all")]
        public async Task<List<BrandDto>> Get()
        {
            return await brandsService.GetAll();
        }

        [HttpGet("{id}")]
        public async Task<BrandDto> Get(int id)
        {
            return await brandsService.GetById(id);
        }

        [HttpPost("add")]
        [Authorize(Roles = "admin")]
        public async Task<BrandDto> Post(BrandDto brand)
        {
            return await brandsService.Add(brand);
        }

        [HttpPut("update/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<BrandDto> Put(int id, [FromBody] BrandDto brand)
        {
            return await brandsService.Update(id, brand);
        }

        [HttpDelete("delete/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<BrandDto> Delete(int id)
        {
            return await brandsService.Delete(id);
        }
    }
}