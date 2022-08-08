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
        private DataContext dataBase;

        public OrdersService(DataContext dataBase)
        {
            this.dataBase = dataBase;
        }

        public async Task<OrderDto> MakeOrder(string userEmail, long[] productSerials)
        {
                User user = dataBase.Users.FirstOrDefault(u => u.Email == userEmail);
            if (user is null)
                return null;

            Order order = new()
            {
                CreationDate = DateTime.Now,
                User = user,
                ProductsList = new()
            };
            foreach (long productSerial in productSerials)
            {
                order.ProductsList.Add(dataBase.Products.FirstOrDefault(p => p.ProductSerial == productSerial));
            }

            dataBase.Orders.Add(order);
            dataBase.SaveChanges();

            return await Task.FromResult(new OrderDto()
            {
                CreationDate = order.CreationDate,
                User = new UserInfoDto()
                {
                    Id = user.Id,
                    Email = user.Email,
                    Role = user.Role,
                    UserName = user.UserName
                }
            });
        }

        public async Task<OrderDto> DeleteOrder(int id)
        {
            Order order = dataBase.Orders.FirstOrDefault(o => o.Id == id);
            dataBase.Orders.Remove(order);
            dataBase.SaveChanges();
            return await Task.FromResult(new OrderDto
            {
                Id = order.Id, CreationDate = order.CreationDate   
            });
        }

        public async Task<List<OrderDto>> GetAll()
        {
            List<OrderDto> orders = new();
            List<Order> query = dataBase.Orders.Include(o => o.User).ToList();
            foreach (Order order in query)
            {
                User user = dataBase.Users.FirstOrDefault(u => u.Email == order.User.Email);
                List<ProductDto> orderProducts = new();
                foreach (Product product in dataBase.Products.Include(p => p.Brand).ToList())
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
                        Email = user.Email,
                        UserName = user.UserName,
                        Role = user.Role
                    },
                    ProductsList = orderProducts
                });
            }
            return await Task.FromResult(orders);
        }
    }
}
