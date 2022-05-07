using Entities;
using System.Collections.Generic;
using System.Linq;
using WebApi.DTO;

namespace WebApi.Services
{
    public class UsersService
    {
        private DataContext dataBase;

        public UsersService(DataContext dataBase)
        {
            this.dataBase = dataBase;
        }

        public IEnumerable<UserDto> GetAll()
        {
            List<UserDto> result = new List<UserDto>();
            foreach (User user in dataBase.Users)
            {
                result.Add(new UserDto() { Id = user.Id, Email = user.Email, Role = user.Role, UserName = user.UserName });
            }
            return result;
        }

        public UserDto Get(int id)
        {
            User user = dataBase.Users.FirstOrDefault(u => u.Id == id);
            UserDto result = new UserDto()
            {
                Id = user.Id,
                Email = user.Email,
                Role = user.Role,
                UserName = user.UserName
            };
            return result;
        }

        public UserDto Post(UserDto user)
        {
            User result = new User()
            {
                UserName = user.UserName,
                Email = user.Email,
                Role = user.Role
            };
            dataBase.Users.Add(result);
            return user;
        }
    }
}