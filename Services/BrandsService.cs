using Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApi.DTO;

namespace WebApi.Services
{
    public class BrandsService
    {
        private DataContext dataBase;

        public BrandsService(DataContext dataBase)
        {
            this.dataBase = dataBase;
        }

        public async Task<List<BrandDto>> GetAll()
        {
            List<BrandDto> brands = new();
            foreach (Brand brand in dataBase.Brands.Include(b => b.Products))
            {
                List<ProductDto> products = new();
                foreach (Product product in brand.Products)
                {
                    products.Add(new ProductDto()
                    {
                        Id = product.Id,
                        Name = product.Name,
                        BrandName = product.Brand.Name,
                        Price = product.Price,
                        ProductSerial = product.ProductSerial
                    });
                }
                brands.Add(new BrandDto()
                {
                    Id = brand.Id,
                    Name = brand.Name,
                    Specialization = brand.Specialization,
                    Products = products
                });
            }
            return await Task.FromResult(brands);
        }

        public async Task<BrandDto> Get(int id)
        {
            Brand brand = dataBase.Brands.Include(b => b.Products).FirstOrDefault(b => b.Id == id);
            List<ProductDto> products = new();
            foreach (Product product in brand.Products)
            {
                products.Add(new ProductDto()
                {
                    Id = product.Id,
                    Name = product.Name,
                    BrandName = product.Brand.Name,
                    Price = product.Price,
                    ProductSerial = product.ProductSerial
                });
            }
            return await Task.FromResult(new BrandDto()
            {
                Id = brand.Id,
                Name = brand.Name,
                Specialization = brand.Specialization,
                Products = products
            });
        }

        public async Task<BrandDto> Post(BrandDto brandDto)
        {
            Brand brand = new()
            {
                Name = brandDto.Name,
                Specialization = brandDto.Specialization
            };
            dataBase.Brands.Add(brand);
            dataBase.SaveChanges();

            brandDto.Id = dataBase.Brands.First(b => b.Name == brand.Name).Id;
            return await Task.FromResult(brandDto);
        }

        public async Task<BrandDto> Put(int id, BrandDto dto)
        {
            Brand result = dataBase.Brands.FirstOrDefault(b => b.Id == id);
            result.Specialization = dto.Specialization;
            result.Name = dto.Name;
            dataBase.SaveChanges();
            return await Task.FromResult(dto);
        }

        public async Task<BrandDto> Delete(int id)
        {
            Brand result = dataBase.Brands.FirstOrDefault(p => p.Id == id);
            dataBase.Brands.Remove(result);
            dataBase.SaveChanges();
            return await Task.FromResult(new BrandDto
            {
                Id = result.Id,
                Name = result.Name,
                Specialization = result.Specialization
            });
        }
    }
}