using Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using WebApi.DTO;

namespace WebApi.Services
{
    public class ProductsService
    {
        private DataContext dataBase;

        public ProductsService(DataContext dataBase)
        {
            this.dataBase = dataBase;
        }

        public IEnumerable<ProductDto> GetAll()
        {
            List<ProductDto> result = new List<ProductDto>();
            foreach (Product product in dataBase.Products.Include(p => p.Brand))
            {
                result.Add(new ProductDto()
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    ProductSerial = product.ProductSerial,
                    BrandName = product.Brand.Name
                });
            }
            return result;
        }

        public ProductDto Get(int id)
        {
            Product product = dataBase.Products.Include(p => p.Brand).FirstOrDefault(p => p.Id == id);
            return new ProductDto()
            {
                Id = product.Id,
                Name = product.Name,
                BrandName = product.Brand.Name,
                Price = product.Price,
                ProductSerial = product.ProductSerial
            };
        }

        public ProductDto Post(ProductDto product)
        {
            Product result = new Product()
            {
                ProductSerial = product.ProductSerial,
                Name = product.Name,
                Price = product.Price,
                Brand = dataBase.Brands.FirstOrDefault(b => b.Name == product.BrandName)
            };
            dataBase.Products.Add(result);
            dataBase.SaveChanges();
            return product;
        }

        public ProductDto Put(int id, ProductDto product)
        {
            Product result = dataBase.Products.FirstOrDefault(p => p.Id == id);
            Brand brand = dataBase.Brands.FirstOrDefault(b => product.BrandName == b.Name);
            result.Name = product.Name;
            result.Price = product.Price;
            result.ProductSerial = product.ProductSerial;
            result.Brand = brand;
            dataBase.SaveChanges();
            return product;
        }

        public ProductDto Delete(int id)
        {
            Product result = dataBase.Products.FirstOrDefault(p => p.Id == id);
            dataBase.Products.Remove(result);
            dataBase.SaveChanges();
            return new ProductDto
            {
                Id = result.Id,
                BrandName = result.Brand.Name,
                Name = result.Name,
                Price = result.Price,
                ProductSerial = result.ProductSerial
            };
        }
    }
}