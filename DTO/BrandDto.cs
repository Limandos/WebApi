using System.Collections.Generic;

namespace WebApi.DTO
{
    public class BrandDto : BaseDto
    {
        public string Name { get; set; }

        public string Specialization { get; set; }

        public List<ProductDto> Products { get; set; }
    }
}