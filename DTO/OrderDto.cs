using System;
using System.Collections.Generic;

namespace WebApi.DTO
{
    public class OrderDto : BaseDto
    {
        public UserInfoDto User { get; set; }

        public DateTime CreationDate { get; set; }

        public List<ProductDto> ProductsList { get; set; }
    }
}