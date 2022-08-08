using Entities;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WebApi.DTO;

namespace WebApi.Services
{
    public class UsersService
    {
        private DataContext dataBase;
        private const string KEY = "!$uper$eCret4ey!=+1242lol";

        public UsersService(DataContext dataBase)
        {
            this.dataBase = dataBase;
        }

        public async Task<List<UserInfoDto>> GetAll()
        {
            List<UserInfoDto> result = new();
            foreach (User user in dataBase.Users)
            {
                result.Add(new UserInfoDto() { Id = user.Id, Email = user.Email, Role = user.Role, UserName = user.UserName });
            }
            return await Task.FromResult(result);
        }

        public async Task<UserInfoDto> GetById(int id)
        {
            User user = dataBase.Users.FirstOrDefault(u => u.Id == id);
            UserInfoDto result = new()
            {
                Id = user.Id,
                Email = user.Email,
                Role = user.Role,
                UserName = user.UserName
            };
            return await Task.FromResult(result);
        }

        public async Task<string> Register(UserRegisterDto userRegisterDto)
        {
            CreatePassword(userRegisterDto.Password, out byte[] passwordHash, out byte[] passwordSalt);

            User user = new()
            {
                UserName = userRegisterDto.UserName,
                Email = userRegisterDto.Email,
                Role = "User",
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            dataBase.Users.Add(user);
            dataBase.SaveChanges();
            return await Task.FromResult(CreateToken(user));
        }

        public async Task<string> Login(UserLoginDto userLoginDto)
        {
            User user = dataBase.Users.Where(user => user.Email == userLoginDto.Email).FirstOrDefault();

            if (user is null)
                return null;

            if (!VerifyPassword(userLoginDto, user))
                return null;

            return await Task.FromResult(CreateToken(user));
        }

        private void CreatePassword(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (HMACSHA256 hmac = new())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPassword(UserLoginDto userLoginDto, User user)
        {
            using (var hmac = new HMACSHA256(user.PasswordSalt))
            {
                byte[] loginHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(userLoginDto.Password));
                return loginHash.SequenceEqual(user.PasswordHash);
            }
        }

        private string CreateToken(User user)
        {
            var claimList = new List<Claim>
            {
                new Claim("email", user.Email),
                new Claim("role", user.Role)
            };
            var token = new JwtSecurityToken(
                claims: claimList,
                expires: DateTime.UtcNow.Add(TimeSpan.FromHours(1)),
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY)), SecurityAlgorithms.HmacSha256)
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}