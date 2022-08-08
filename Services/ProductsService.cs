using Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<List<ProductDto>> GetAll()
        {
            List<ProductDto> result = new();
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
            return await Task.FromResult(result);
        }

        public async Task<ProductDto> Get(int id)
        {
            Product product = dataBase.Products.Include(p => p.Brand).FirstOrDefault(p => p.Id == id);
            if (product is null)
                return null;

            return await Task.FromResult(new ProductDto()
            {
                Id = product.Id,
                Name = product.Name,
                BrandName = product.Brand.Name,
                Price = product.Price,
                ProductSerial = product.ProductSerial
            });
        }

        public async Task<ProductDto> Post(ProductDto productDto)
        {
            Product product = new()
            {
                ProductSerial = productDto.ProductSerial,
                Name = productDto.Name,
                Price = productDto.Price,
                Brand = dataBase.Brands.FirstOrDefault(b => b.Name == productDto.BrandName)
            };
            dataBase.Products.Add(product);
            dataBase.SaveChanges();

            productDto.Id = dataBase.Products.First(p => p.ProductSerial == product.ProductSerial).Id;
            return await Task.FromResult(productDto);
        }

        public async Task<ProductDto> Put(int id, ProductDto product)
        {
            Product result = dataBase.Products.FirstOrDefault(p => p.Id == id);
            Brand brand = dataBase.Brands.FirstOrDefault(b => product.BrandName == b.Name);
            result.Name = product.Name;
            result.Price = product.Price;
            result.ProductSerial = product.ProductSerial;
            result.Brand = brand;
            dataBase.SaveChanges();
            return await Task.FromResult(product);
        }

        public async Task<ProductDto> Delete(int id)
        {
            Product result = dataBase.Products.Include(p => p.Brand).FirstOrDefault(p => p.Id == id);
            dataBase.Products.Remove(result);
            dataBase.SaveChanges();
            return await Task.FromResult(new ProductDto
            {
                Id = result.Id,
                BrandName = result.Brand.Name,
                Name = result.Name,
                Price = result.Price,
                ProductSerial = result.ProductSerial
            });
        }
    }
}