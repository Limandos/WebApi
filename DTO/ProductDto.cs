namespace WebApi.DTO
{
    public class ProductDto : BaseDto
    {
        public string Name { get; set; }

        public long ProductSerial { get; set; }

        public int Price { get; set; }

        public string BrandName { get; set; }
    }
}