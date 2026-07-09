using AsyncPlate.Application.DTOs.ProductExtra;
using AsyncPlate.Application.Interfaces.Repositories;
using AsyncPlate.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Infrastructure.Data.Repositories
{
    public class ProductExtraRepo : GenericRepo<ProductExtra>, IProductExtraRepo

    {
        public ProductExtraRepo(AppDbContext context) : base(context)
        {
        }
        public async Task<bool> IsExtraProductRelatedToProduct(string productId, string extraProductId)
        {
            return await _context.ProductExtras.AnyAsync(x => x.ProductId == productId && x.ExtraProductId == extraProductId);
        }

        public async Task<IEnumerable<ProductExtra>> GetByProductIdAsync(string productId)//product productextra
        {
            return await _context.ProductExtras.Where(x => x.ProductId == productId).ToListAsync();
        }

        public async Task<ProductExtra?> GetProductExtraAsync(string productId, string extraProductId)
        {
            return await _context.ProductExtras.FirstOrDefaultAsync(pe => pe.ProductId == productId && pe.ExtraProductId == extraProductId);
        }

        public async Task<IEnumerable<ProductExtraDTO>> GetExtrasByProductIdAsync(string productId)
        {
            //projection no need to include => it works ^_^

            return await _context.ProductExtras.Where(pe => pe.ProductId == productId)
                .Select(pe => new ProductExtraDTO
                {
                    Id = pe.ExtraProduct.Id,
                    Name = pe.ExtraProduct.Name,
                    BasePrice = pe.ExtraProduct.BasePrice,
                }).ToListAsync();
        }
        public async Task<List<ProductExtra>> GetProductExtrasByProductIdsAsync(IEnumerable<string> productIds)
        {
            return await _context.ProductExtras
                .AsNoTracking()
                .Where(pe => productIds.Contains(pe.ProductId))
                .ToListAsync();
        }

    }

}
