using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebApi.DTO;
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

        [HttpGet]
        public ActionResult<IEnumerable<BrandDto>> Get()
        {
            return new ActionResult<IEnumerable<BrandDto>>(brandsService.GetAll());
        }

        [HttpGet("{id}")]
        public ActionResult<BrandDto> Get(int id)
        {
            return new ActionResult<BrandDto>(brandsService.Get(id));
        }

        [HttpPost]
        public ActionResult<BrandDto> Post(BrandDto brand)
        {
            return new ActionResult<BrandDto>(brandsService.Post(brand));
        }

        [HttpPut]
        public ActionResult<BrandDto> Put(int id, [FromBody] BrandDto brand)
        {
            return new ActionResult<BrandDto>(brandsService.Put(id, brand));
        }

        [HttpDelete("{id}")]
        public ActionResult<BrandDto> Delete(int id)
        {
            return new ActionResult<BrandDto>(brandsService.Delete(id));
        }
    }
}