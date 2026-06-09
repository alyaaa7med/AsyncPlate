using AsyncPlate.API.Models;
using AsyncPlate.Application.DTOs.Product;
using AsyncPlate.Application.DTOs.Recipe;
using AsyncPlate.Application.Services.Interfaces;
using AsyncPlate.Application.Services.Implementation;
using Microsoft.AspNetCore.Mvc;

namespace AsyncPlate.API.Controllers
{

    [ApiController]
    [Route("api/[controller]s")]
    //[Authorize(Roles = "Admin")]
    public class ProductController:ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpPost]
        public async Task<IActionResult> AddProduct([FromBody] AddProductRequestDTO productRequestDTO)
        {
            var responseDto = await _productService.AddProductAsync(productRequestDTO);
            return Created($"/products/{responseDto.Id}", new ApiResponse<ProductResponseDTO>(true, "Product created successfully", responseDto));
        }

        [HttpGet("{productId}")]
        public async Task<IActionResult> GetProductById([FromRoute]string productId)
        {
            var responseDto = await _productService.GetProductByIdAsync(productId);
            return Ok(new ApiResponse<ProductResponseDTO>(true, "Product retrieved successfully", responseDto));
        }




        [HttpGet("{productId}/recipes")]
        public async Task<IActionResult> GetRecipesByProductId([FromRoute] string productId)
        {
            var recipeListDTOs = await _productService.GetRecipeByProductIdAsync(productId);
            return Ok(new ApiResponse<IEnumerable<RecipeListDTO>>(true, "Recipes retrieved successfully", recipeListDTOs));
        }
    }
}
