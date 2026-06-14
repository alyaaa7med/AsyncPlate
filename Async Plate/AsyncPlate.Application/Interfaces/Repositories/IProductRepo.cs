using AsyncPlate.Domain.Entities;
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

    }
}
