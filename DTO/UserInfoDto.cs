using System.Collections.Generic;

namespace WebApi.DTO
{
    public class UserInfoDto : BaseDto
    {
        public string UserName { get; set; }

        public string UserEmail { get; set; }

        public string UserRole { get; set; }

        public List<OrderDto> Orders { get; set; }
    }
}