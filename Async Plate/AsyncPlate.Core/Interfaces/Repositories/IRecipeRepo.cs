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
        Task<List<Recipe>> GetRecipeOfProductByIdWithInventoryAsync(string productId);
     }
}
