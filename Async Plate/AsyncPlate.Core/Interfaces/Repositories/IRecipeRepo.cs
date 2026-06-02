using AsyncPlate.Core.DTOs.Recipe;
using AsyncPlate.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Core.Interfaces.Repositories
{
    public interface IRecipeRepo : IBaseRepo<Recipe>
    {
        Task<Recipe?> GetRecipeWithInventoryAndProductAsync(string inventoryId, string productId);

        Task<IEnumerable<RecipeListDTO>> GetRecipeByProductIdAsync(string productId);//i used dto for projection to avoid loading unnecessary data
        IQueryable<RecipeResponseDTO> GetAllRecipes();//i used dto for projection to avoid loading unnecessary data

        Task<IEnumerable<Recipe>> GetRecipeByProductIdAsync2(string productId);

    }
}
