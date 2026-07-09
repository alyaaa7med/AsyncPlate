using AsyncPlate.Application.DTOs.ProductExtra;
using AsyncPlate.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Application.Interfaces.Repositories
{
    public interface IProductRepo : IBaseRepo<Product>
    {
        Task<Product?> GetProductWithCategoryAsync(string productId);
        Task<List<Product>> GetTopSellingProductsAsync();
        Task<List<Product>> GetProductsByCategoryIdAsync(string categoryId);
        Task<List<Product>> GetAvailableProductsAsync();
        Task<List<Product>> GetUnavailableProductsAsync();
        Task<List<Product>> GetProductsByPriceRangeAsync(decimal minPrice, decimal maxPrice);
        IQueryable<Product> GetAllWithCatgeorySummary();
        Task<List<string>> GetByIdsAsync(IEnumerable<string> ids);
        Task<List<Product>> GetProductsByIdsAsync(IEnumerable<string> ids);
        Task<List<string>> GetInvalidExtraProductNamesAsync(IEnumerable<string> productIds);
        Task<ProductWithExtrasDTO?> GetProductWithExtrasAsync(string productId);
        bool HasActiveOffer(Product product);
        decimal GetFinalPrice(Product product);
        IQueryable<Product> GetMenuProducts();
        Task<Product?> GetMenuProductByIdAsync(string productId);

    }
}
