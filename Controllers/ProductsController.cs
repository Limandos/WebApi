using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebApi.Services;
using WebApi.DTO;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductsController : ControllerBase
    {
        private ProductsService productsService;

        public ProductsController(ProductsService productsService)
        {
            this.productsService = productsService;
        }

        [HttpGet("all")]
        public async Task<List<ProductDto>> Get()
        {
            return await productsService.GetAll();
        }

        [HttpGet("{id}")]
        public async Task<ProductDto> Get(int id)
        {
            return await productsService.GetById(id);
        }

        [HttpPost("add")]
        [Authorize(Roles = "admin")]
        public async Task<ProductDto> Post(ProductDto product)
        {
            return await productsService.Add(product);
        }

        [HttpPut("update/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ProductDto> Put(int id, [FromBody] ProductDto product)
        {
            return await productsService.Update(id, product);
        }

        [HttpDelete("delete/{id}")]
        [Authorize(Roles = "admin")]
        public async Task<ProductDto> Delete(int id)
        {
            return await productsService.Delete(id);
        }
    }
}