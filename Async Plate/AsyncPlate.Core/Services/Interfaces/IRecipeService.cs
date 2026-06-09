using AsyncPlate.Application.Common.DTOs;
using AsyncPlate.Application.DTOs.Recipe;
using AsyncPlate.Application.DTOs.Inventory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Application.Services.Interfaces
{
    public interface IRecipeService
    {

        Task<RecipeResponseDTO> GetRecipeByIdAsync(string inventoryId, string productId);
        Task<RecipeResponseDTO> AddRecipeAsync(AddRecipeRequestDTO requestDTO);
        Task<RecipeResponseDTO> UpdateRecipeAsync(string productId, string inventoryId, UpdateRecipeRequestDTO requestDTO);
        Task<RecipeResponseDTO> DeleteRecipeAsync(string inventoryId, string productId);
        Task<PagedResult<RecipeResponseDTO>> GetAllRecipesAsync(RecipeFilterDTO filterDto);

        //Task<RecipeResponseDTO> CookProductAsync( AddRecipeRequestDTO cookRecipeRequestDTO);
    }
}
