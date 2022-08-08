using System.Collections.Generic;

namespace WebApi.DTO
{
    public class UserInfoDto : BaseDto
    {
        public string UserName { get; set; }

        public string Email { get; set; }

        public string Role { get; set; }

        public List<OrderDto> Orders { get; set; }
    }
}