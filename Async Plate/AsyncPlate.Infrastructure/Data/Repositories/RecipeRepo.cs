using AsyncPlate.Application.DTOs;
using AsyncPlate.Application.DTOs.Product;
using AsyncPlate.Application.DTOs.Recipe;
using AsyncPlate.Application.DTOs.Supplier;
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

        public async Task<IEnumerable<RecipeListDTO>> GetRecipeByProductIdAsync(string productId)
        {
            return await _context.Recipes.Where(r => r.ProductId == productId)

                                   .Select(r => new RecipeListDTO
                                   {
                                       InventoryName = r.Inventory.Name,
                                       Quantity = r.Quantity
                                   }).ToListAsync();

        }

        public IQueryable<RecipeResponseDTO> GetAllRecipes()//for projection 
        {
            return _context.Recipes.Select(r => new RecipeResponseDTO
            {
                Product = new ProductSummaryDTO
                {
                    Id = r.Product.Id,
                    Name = r.Product.Name
                },
                Inventory = new InventorySummaryDTO
                {
                    Id = r.Inventory.Id,
                    Name = r.Inventory.Name
                }
                ,
                Quantity = r.Quantity
            });
        }

     

        public async Task<List<Recipe>> GetRecipesByProductIdsAsync(IEnumerable<string> productIds)
        {
            return await _context.Recipes.Where(r => productIds.Contains(r.ProductId)).ToListAsync();
        }
    }
}
