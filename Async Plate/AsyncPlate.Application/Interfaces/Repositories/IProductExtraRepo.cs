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
    public interface IProductExtraRepo : IBaseRepo<ProductExtra>
    {
        Task<bool> IsExtraProductRelatedToProduct(string productId, string extraProductId);
        Task<IEnumerable<ProductExtra>> GetByProductIdAsync(string productId);

        Task<ProductExtra?> GetProductExtraAsync(string productId, string extraProductId);

        Task<IEnumerable<ProductExtraDTO>> GetExtrasByProductIdAsync(string productId);

    }
}
