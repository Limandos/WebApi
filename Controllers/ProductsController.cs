using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebApi.Services;
using WebApi.DTO;

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

        [HttpGet]
        public ActionResult<IEnumerable<ProductDto>> Get() => new ActionResult<IEnumerable<ProductDto>>(productsService.GetAll());

        [HttpGet("{id}")]
        public ActionResult<ProductDto> Get(int id)
        {
            return new ActionResult<ProductDto>(productsService.Get(id));
        }

        [HttpPost]
        public ActionResult<ProductDto> Post(ProductDto product)
        {
            return new ActionResult<ProductDto>(productsService.Post(product));
        }

        [HttpPut]
        public ActionResult<ProductDto> Put(int id, [FromBody] ProductDto product)
        {
            return new ActionResult<ProductDto>(productsService.Put(id, product));
        }

        [HttpDelete("{id}")]
        public ActionResult<ProductDto> Delete(int id)
        {
            return new ActionResult<ProductDto>(productsService.Delete(id));
        }
    }
}