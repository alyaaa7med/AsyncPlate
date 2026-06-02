using AsyncPlate.Core.Entities;
using AsyncPlate.Core.Interfaces.Repositories;
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
    }
}
