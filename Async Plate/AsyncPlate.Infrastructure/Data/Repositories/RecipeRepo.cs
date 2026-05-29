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
    public class RecipeRepo : GenericRepo<Recipe>, IRecipeRepo
    {
        public RecipeRepo(AppDbContext context) : base(context)
        {
            // create / update / get / delete are in the generic repo no need to re-implement them here
        }

        public async Task<Recipe?> GetRecipeWithInventoryAndProductAsync(string inventoryId, string productId)
        {
            return await _context.Recipes
                                   .Include(r => r.Product)
                                   .Include(r => r.Inventory)
                                   .SingleOrDefaultAsync(r => r.InventoryId == inventoryId && r.ProductId == productId);
        }
        public async Task<List<Recipe>> GetRecipeOfProductByIdWithInventoryAsync(string productId)
        {
            return await _context.Recipes
                                   .Include(r => r.Product)
                                   .Include(r => r.Inventory)
                                   .Where(r => r.ProductId == productId)
                                   .ToListAsync();
        }
    }
}
