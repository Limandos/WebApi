using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
using WebApi.Exceptions;

namespace WebApi.Services
{
    public class UsersService
    {
        private readonly DataContext dataBase;
        private readonly IConfiguration configuration;

        public UsersService(DataContext dataBase, IConfiguration configuration)
        {
            this.dataBase = dataBase;
            this.configuration = configuration;
        }

        public async Task<List<UserInfoDto>> GetAll()
        {
            List<UserInfoDto> result = new();
            foreach (User user in await dataBase.Users.ToListAsync())
            {
                result.Add(new UserInfoDto() { Id = user.Id, UserEmail = user.Email, UserRole = user.Role, UserName = user.UserName });
            }
            return result;
        }

        public async Task<UserInfoDto> GetById(int id)
        {
            User user = await dataBase.Users.FirstOrDefaultAsync(u => u.Id == id);
            UserInfoDto result = new()
            {
                Id = user.Id,
                UserEmail = user.Email,
                UserRole = user.Role,
                UserName = user.UserName
            };
            return result;
        }

        public async Task<string> Register(UserRegisterDto userRegisterDto)
        {
            CreatePassword(userRegisterDto.UserPassword, out byte[] passwordHash, out byte[] passwordSalt);

            User user = new()
            {
                UserName = userRegisterDto.UserName,
                Email = userRegisterDto.UserEmail,
                Role = "User",
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            dataBase.Users.Add(user);
            await dataBase.SaveChangesAsync();
            return CreateToken(user);
        }

        public async Task<string> Login(UserLoginDto userLoginDto)
        {
            User user = await dataBase.Users.Where(user => user.Email == userLoginDto.UserEmail).FirstOrDefaultAsync();

            if (user is null)
                throw new NotFoundException("User wasn't found.", userLoginDto.UserEmail);

            if (!VerifyPassword(userLoginDto, user))
                return null;

            return CreateToken(user);
        }

        private static void CreatePassword(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            HMACSHA256 hmac = new();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        private static bool VerifyPassword(UserLoginDto userLoginDto, User user)
        {
            HMACSHA256 hmac = new(user.PasswordSalt);
            byte[] loginHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(userLoginDto.UserPassword));
            return loginHash.SequenceEqual(user.PasswordHash);
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
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("AppSettings:JWTKey").Value)), SecurityAlgorithms.HmacSha256)
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}