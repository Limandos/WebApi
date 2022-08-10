using Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebApi.DTO;
using WebApi.Exceptions;

namespace WebApi.Services
{
    public class BrandsService
    {
        private readonly DataContext dataBase;

        public BrandsService(DataContext dataBase)
        {
            this.dataBase = dataBase;
        }

        public async Task<List<BrandDto>> GetAll()
        {
            List<BrandDto> brandDtos = new();
            foreach (Brand brand in await dataBase.Brands.Include(b => b.Products).ToListAsync())
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

                brandDtos.Add(new BrandDto()
                {
                    Id = brand.Id,
                    Name = brand.Name,
                    Specialization = brand.Specialization,
                    Products = products
                });
            }
            return brandDtos;
        }

        public async Task<BrandDto> GetById(int id)
        {
            Brand brand = await dataBase.Brands.Include(b => b.Products).FirstOrDefaultAsync(b => b.Id == id);
            if (brand is null)
                throw new NotFoundException("Brand not found!", id.ToString());

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

            return new BrandDto()
            {
                Id = brand.Id,
                Name = brand.Name,
                Specialization = brand.Specialization,
                Products = products
            };
        }

        public async Task<BrandDto> Add(BrandDto brandDto)
        {
            Brand brand = new()
            {
                Name = brandDto.Name,
                Specialization = brandDto.Specialization
            };
            dataBase.Brands.Add(brand);
            await dataBase.SaveChangesAsync();

            brandDto.Id = (await dataBase.Brands.FirstAsync(b => b.Name == brand.Name)).Id;
            return brandDto;
        }

        public async Task<BrandDto> Update(int id, BrandDto brandDto)
        {
            Brand brand = await dataBase.Brands.FirstOrDefaultAsync(b => b.Id == id);
            if (brand is null)
                throw new NotFoundException("Brand not found!", id.ToString());

            brand.Specialization = brandDto.Specialization;
            brand.Name = brandDto.Name;
            await dataBase.SaveChangesAsync();
            return brandDto;
        }

        public async Task<BrandDto> Delete(int id)
        {
            Brand result = await dataBase.Brands.FirstOrDefaultAsync(p => p.Id == id);
            if (result is null)
                throw new NotFoundException("Brand not found!", id.ToString());

            dataBase.Brands.Remove(result);
            await dataBase.SaveChangesAsync();
            return new BrandDto
            {
                Id = result.Id,
                Name = result.Name,
                Specialization = result.Specialization
            };
        }
    }
}