using AsyncPlate.Application.Common.DTOs;
using AsyncPlate.Application.Common.Extenstions;
using AsyncPlate.Application.DTOs.Product;
using AsyncPlate.Application.DTOs.ProductExtra;
using AsyncPlate.Application.Interfaces.Repositories;
using AsyncPlate.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Type = AsyncPlate.Domain.Entities.Type;

namespace AsyncPlate.Infrastructure.Data.Repositories
{
    public class ProductRepo : GenericRepo<Product>, IProductRepo
    {
        public ProductRepo(AppDbContext context) : base(context)
        {
        }

        public async Task<Product?> GetProductWithCategoryAsync(string productId)
        {
            return await _context.Products
                .AsNoTracking()
                .Include(p => p.Category)
                .SingleOrDefaultAsync(p => p.Id == productId);
        }

        public async Task<List<Product>> GetTopSellingProductsAsync()
        {
            return await _context.Products
                .OrderByDescending(p => p.TotalTimesOrdered)
                .Take(5)
                .Include(p => p.Category)
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<List<Product>> GetProductsByCategoryIdAsync(string categoryId)
        {
            return await _context.Products
                .AsNoTracking()
                .Where(p => p.CategoryId == categoryId)
                .Include(p => p.Category)
                .ToListAsync();


        }

        public async Task<List<Product>> GetAvailableProductsAsync()
        {
            return await _context.Products
               .AsNoTracking()
               .Where(p => p.IsAvailable == true)
               .Include(p => p.Category)
               .ToListAsync();

        }

        public async Task<List<Product>> GetUnavailableProductsAsync()
        {

            return await _context.Products
               .AsNoTracking()
               .Where(p => p.IsAvailable == false)
               .Include(p => p.Category)
               .ToListAsync();

        }

        public async Task<List<Product>> GetProductsByPriceRangeAsync(decimal minPrice, decimal maxPrice)
        {
            return await _context.Products
                .AsNoTracking()
                .Where(p => p.BasePrice >= minPrice && p.BasePrice <= maxPrice)
                .Include(p => p.Category)
                .ToListAsync();
        }

        public IQueryable<Product> GetAllWithCatgeorySummary()
        {
            return _context.Products
                .AsNoTracking()
                .Include(p => p.Category);
        }

        public async Task<List<string>> GetByIdsAsync(IEnumerable<string> ids)
        {
            return await _context.Products.AsNoTracking()
                .Where(p => ids.Contains(p.Id)).Select(p => p.Id).ToListAsync();
        }
        public async Task<List<string>> GetInvalidExtraProductNamesAsync(IEnumerable<string> productIds)
        {
            return await _context.Products
                .AsNoTracking()
                .Where(p => productIds.Contains(p.Id) && p.Type != Type.Extra)
                .Select(p => p.Name)
                .ToListAsync();
        }

        public async Task<ProductWithExtrasDTO?> GetProductWithExtrasAsync(string productId)
        {

            return await _context.Products
                                 .Where(p => p.Id == productId)
                                 .Select(p => new ProductWithExtrasDTO
                                 {
                                     Id = p.Id,
                                     Name = p.Name,
                                     BasePrice = p.BasePrice,

                                     Extras = p.MainProducts.Select(pe => new ProductExtraDTO{
                                                             Id = pe.ExtraProductId,
                                                             Name = pe.ExtraProduct.Name,
                                                             BasePrice = pe.ExtraProduct.BasePrice
                                 }).ToList()
                                 }).FirstOrDefaultAsync();
        }

        public IQueryable<Product> GetMenuProducts()
        {
            return _context.Products
                .AsNoTracking()
                .Include(x => x.Category)
                    .ThenInclude(c => c.CurrentOffer)
                .Include(x => x.ExtraProducts)
                    .ThenInclude(pe => pe.ExtraProduct)
                .Include(x => x.Recipes)
                    .ThenInclude(r => r.Inventory);
        }

        public async Task<Product?> GetMenuProductByIdAsync(string productId)
        {
            return await _context.Products
                .AsNoTracking()
                .Include(p => p.Category)
                    .ThenInclude(c => c.CurrentOffer)
                .Include(p => p.ExtraProducts)
                    .ThenInclude(pe => pe.ExtraProduct)
                .Include(p => p.Recipes)
                    .ThenInclude(r => r.Inventory)
                .FirstOrDefaultAsync(p => p.Id == productId);
        }
        public bool HasActiveOffer(Product product)
        {
            var offer = product.Category?.CurrentOffer;

            return offer != null && offer.IsActive && offer.StartDate <= DateTime.UtcNow && offer.EndDate >= DateTime.UtcNow;
        }
        public decimal GetFinalPrice(Product product)
        {
            if (!HasActiveOffer(product))
                return product.BasePrice;

            return product.BasePrice * (1 - product.Category.CurrentOffer!.DiscountPercentage / 100m);
        }
    }
}