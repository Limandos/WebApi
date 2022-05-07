using System.Collections.Generic;

namespace WebApi.DTO
{
    public class BrandDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string HeadQuarters { get; set; }

        public List<ProductDto> Products { get; set; }
    }
}