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
        private readonly DataContext dataBase;

        public ProductsService(DataContext dataBase)
        {
            this.dataBase = dataBase;
        }

        public async Task<List<ProductDto>> GetAll()
        {
            List<ProductDto> result = new();
            foreach (Product product in await dataBase.Products.Include(p => p.Brand).ToListAsync())
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

        public async Task<ProductDto> GetById(int id)
        {
            Product product = await dataBase.Products.Include(p => p.Brand).FirstOrDefaultAsync(p => p.Id == id);
            if (product is null)
                return null;

            return new ProductDto()
            {
                Id = product.Id,
                Name = product.Name,
                BrandName = product.Brand.Name,
                Price = product.Price,
                ProductSerial = product.ProductSerial
            };
        }

        public async Task<ProductDto> Add(ProductDto productDto)
        {
            Product product = new()
            {
                ProductSerial = productDto.ProductSerial,
                Name = productDto.Name,
                Price = productDto.Price,
                Brand = await dataBase.Brands.FirstOrDefaultAsync(b => b.Name == productDto.BrandName)
            };
            dataBase.Products.Add(product);
            await dataBase.SaveChangesAsync();

            productDto.Id = (await dataBase.Products.FirstAsync(p => p.ProductSerial == product.ProductSerial)).Id;
            return productDto;
        }

        public async Task<ProductDto> Update(int id, ProductDto productDto)
        {
            Product product = await dataBase.Products.FirstOrDefaultAsync(p => p.Id == id);
            Brand brand = await dataBase.Brands.FirstOrDefaultAsync(b => productDto.BrandName == b.Name);
            product.Name = productDto.Name;
            product.Price = productDto.Price;
            product.ProductSerial = productDto.ProductSerial;
            product.Brand = brand;
            dataBase.SaveChanges();
            return productDto;
        }

        public async Task<ProductDto> Delete(int id)
        {
            Product result = await dataBase.Products.Include(p => p.Brand).FirstOrDefaultAsync(p => p.Id == id);
            dataBase.Products.Remove(result);
            await dataBase.SaveChangesAsync();
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