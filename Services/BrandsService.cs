using Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
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

        public IEnumerable<BrandDto> GetAll()
        {
            List<BrandDto> result = new List<BrandDto>();
            foreach (Brand brand in dataBase.Brands.Include(b => b.Products))
            {
                List<ProductDto> products = new List<ProductDto>();
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
                result.Add(new BrandDto()
                {
                    Id = brand.Id,
                    Name = brand.Name,
                    HeadQuarters = brand.HeadQuarters,
                    Products = products
                });
            }
            return result;
        }

        public BrandDto Get(int id)
        {
            Brand brand = dataBase.Brands.Include(b => b.Products).FirstOrDefault(b => b.Id == id);
            List<ProductDto> products = new List<ProductDto>();
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
                HeadQuarters = brand.HeadQuarters,
                Products = products
            };
        }

        public BrandDto Post(BrandDto brand)
        {
            Brand result = new Brand()
            {
                Name = brand.Name,
                HeadQuarters = brand.HeadQuarters
            };
            dataBase.Brands.Add(result);
            dataBase.SaveChanges();
            return brand;
        }

        public BrandDto Put(int id, BrandDto dto)
        {
            Brand result = dataBase.Brands.FirstOrDefault(b => b.Id == id);
            result.HeadQuarters = dto.HeadQuarters;
            result.Name = dto.Name;
            dataBase.SaveChanges();
            return dto;
        }

        public BrandDto Delete(int id)
        {
            Brand result = dataBase.Brands.FirstOrDefault(p => p.Id == id);
            dataBase.Brands.Remove(result);
            dataBase.SaveChanges();
            return new BrandDto
            {
                Id = result.Id,
                Name = result.Name,
                HeadQuarters = result.HeadQuarters
            };
        }
    }
}