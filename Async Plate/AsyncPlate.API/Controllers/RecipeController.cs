using AsyncPlate.API.Models;
using AsyncPlate.Core.Common.DTOs;
using AsyncPlate.Core.DTOs.Recipe;
using AsyncPlate.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace AsyncPlate.API.Controllers
{

    [ApiController]
    [Route("api/[controller]s")]
    //[Authorize(Roles = "KitchenChef")]
    public class RecipeController : Controller
    {
        private readonly IRecipeService recipeService;
        public RecipeController(IRecipeService recipeService)
        {
            this.recipeService = recipeService;
        }

        [HttpPost()]
        public async Task<IActionResult> AddRecipe(AddRecipeRequestDTO addRecipeRequestDTO)
        {
            var recipeResponseDTO = await recipeService.AddRecipeAsync(addRecipeRequestDTO);
            return Created($"/recipes/{recipeResponseDTO.Inventory.Id+recipeResponseDTO.Product.Id}", new ApiResponse<RecipeResponseDTO>(true, "Recipe added successfully", recipeResponseDTO));
        }

        [HttpPut("{productId}/{inventoryId}")]
        public async Task<IActionResult> UpdateRecipe(string productId, string inventoryId, UpdateRecipeRequestDTO updateRecipeRequestDTO)
        {
            var recipeResponseDTO = await recipeService.UpdateRecipeAsync(inventoryId, productId, updateRecipeRequestDTO);
            return Ok(new ApiResponse<RecipeResponseDTO>(true, "Recipe updated successfully", recipeResponseDTO));
        }

        [HttpDelete("{productId}/{inventoryId}")]
        public async Task<IActionResult> DeleteRecipe(string productId, string inventoryId)
        {
            var recipeResponseDTO = await recipeService.DeleteRecipeAsync(inventoryId, productId);
            return Ok(new ApiResponse<RecipeResponseDTO>(true, "Recipe deleted successfully", recipeResponseDTO));
        }

        [HttpGet("{productId}/{inventoryId}")]
        public async Task<IActionResult> GetRecipeById(string productId, string inventoryId)
        {
            var recipeResponseDTO = await recipeService.GetRecipeByIdAsync(inventoryId, productId);
            return Ok(new ApiResponse<RecipeResponseDTO>(true, "Recipe retrieved successfully", recipeResponseDTO));
        }


        [HttpGet()]
        public async Task<IActionResult> GetAllRecipes([FromQuery] RecipeFilterDTO filterDto)
        {
            var pagedResult = await recipeService.GetAllRecipesAsync(filterDto);
            return Ok(new ApiResponse<PagedResult<RecipeResponseDTO>>(true, "Recipes retrieved successfully", pagedResult));
        }

        //[HttpPost("cook")] //automatic calling not by any one 
        ////it should be in the order service as we cook when the order is confirmed not when 
        ////the recipe is created 
        //public async Task<IActionResult> CookProduct(AddRecipeRequestDTO cookRecipeRequestDTO)
        //{
        //    await recipeService.MakeRecipeAsync(makeRecipeRequestDTO);
        //    return Ok();
        //}
    }
}
