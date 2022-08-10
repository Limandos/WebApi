using Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.DTO;

namespace WebApi.Services
{
    public class OrdersService
    {
        private readonly DataContext dataBase;

        public OrdersService(DataContext dataBase)
        {
            this.dataBase = dataBase;
        }

        public async Task<OrderDto> MakeOrder(string userEmail, long[] productIds)
        {
            User user = await dataBase.Users.FirstOrDefaultAsync(u => u.Email == userEmail);
            if (user is null)
                return null;

            Order order = new()
            {
                CreationDate = DateTime.Now,
                User = user,
                ProductsList = new()
            };
            foreach (long productId in productIds)
            {
                order.ProductsList.Add(dataBase.Products.FirstOrDefault(p => p.Id == productId));
            }

            dataBase.Orders.Add(order);
            await dataBase.SaveChangesAsync();

            return new OrderDto()
            {
                CreationDate = order.CreationDate,
                User = new UserInfoDto()
                {
                    Id = user.Id,
                    UserEmail = user.Email,
                    UserRole = user.Role,
                    UserName = user.UserName
                }
            };
        }

        public async Task<OrderDto> DeleteOrder(int id)
        {
            Order order = await dataBase.Orders.FirstOrDefaultAsync(o => o.Id == id);
            dataBase.Orders.Remove(order);
            await dataBase.SaveChangesAsync();
            return new OrderDto
            {
                Id = order.Id,
                CreationDate = order.CreationDate
            };
        }

        public async Task<List<OrderDto>> GetAll()
        {
            List<OrderDto> orders = new();
            List<Order> query = await dataBase.Orders.Include(o => o.User).ToListAsync();
            foreach (Order order in query)
            {
                User user = await dataBase.Users.FirstOrDefaultAsync(u => u.Email == order.User.Email);
                List<ProductDto> orderProducts = new();
                foreach (Product product in await dataBase.Products.Include(p => p.Brand).ToListAsync())
                {
                    orderProducts.Add(new()
                    {
                        Id = product.Id,
                        BrandName = product.Brand.Name,
                        Name = product.Name,
                        Price = product.Price,
                        ProductSerial = product.ProductSerial
                    });
                }
                orders.Add(new OrderDto()
                {
                    Id = order.Id,
                    CreationDate = order.CreationDate,
                    User = new UserInfoDto()
                    {
                        Id = user.Id,
                        UserEmail = user.Email,
                        UserName = user.UserName,
                        UserRole = user.Role
                    },
                    ProductsList = orderProducts
                });
            }
            return orders;
        }
    }
}
