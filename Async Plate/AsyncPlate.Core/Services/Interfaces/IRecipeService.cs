using AsyncPlate.Core.Common.DTOs;
using AsyncPlate.Core.DTOs.Inventory;
using AsyncPlate.Core.DTOs.Recipe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Core.Services.Interfaces
{
    public interface IRecipeService
    {

        Task<PagedResult<RecipeResponseDTO>> GetAllRecipesAsync(RecipeFilterDTO filterDto);
        Task<RecipeResponseDTO> GetRecipeByIdAsync(string inventoryId, string productId);
        Task<RecipeResponseDTO> AddRecipeAsync(AddRecipeRequestDTO requestDTO);
        Task<RecipeResponseDTO> UpdateRecipeAsync(string productId, string inventoryId, UpdateRecipeRequestDTO requestDTO);
        Task<RecipeResponseDTO> DeleteRecipeAsync(string inventoryId, string productId);

        //make a recipe 
        //Task<RecipeResponseDTO> CookProductAsync( AddRecipeRequestDTO cookRecipeRequestDTO);
    }
}
