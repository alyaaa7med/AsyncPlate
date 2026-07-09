using AsyncPlate.API.Models;
using AsyncPlate.Application.Common.DTOs;
using AsyncPlate.Application.DTOs.Product;
using AsyncPlate.Application.DTOs.Recipe;
using AsyncPlate.Application.Services.Implementation;
using AsyncPlate.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AsyncPlate.API.Controllers
{

    [ApiController]
    [Route("api/[controller]s")]
    [Authorize(Roles = "Admin")]
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


        [HttpPatch("{productId}/not-available")]
        public async Task<IActionResult> UnavalibleProduct([FromRoute] string productId)
        {
           await _productService.MakeProductUnAvailableAsync(productId);
            return Ok(new ApiResponse<ProductResponseDTO>( true,"Product availability changed successfully",null));
        }

        
        [HttpDelete("{productId}")]
        public async Task<IActionResult> DeleteProduct([FromRoute] string productId)
        {
            await _productService.DeleteProductAsync(productId);

            return Ok(new ApiResponse<ProductResponseDTO>(true, "Product deleted successfully",null));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts([FromQuery] int pageNumber = 1,[FromQuery] int pageSize = 10)
        {
            var responseDto = await _productService.GetAllProductsAsync(pageNumber, pageSize);

            return Ok(new ApiResponse<PagedResult<ProductResponseDTO>>(true,"Products retrieved successfully", responseDto));
        }

       
        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetProductsByCategory([FromRoute] string categoryId)
        {
            var responseDto = await _productService.GetProductsByCategoryAsync(categoryId);

            return Ok(new ApiResponse<IEnumerable<ProductResponseDTO>>(true,"Products retrieved successfully", responseDto));
        }

        
        [HttpGet("available")]
        public async Task<IActionResult> GetAvailableProducts()
        {
            var responseDto = await _productService.GetAvailableProductsAsync();

            return Ok(new ApiResponse<IEnumerable<ProductResponseDTO>>( true, "Available products retrieved successfully",responseDto));
        }

        [HttpGet("unavailable")]
        public async Task<IActionResult> GetUnavailableProducts()
        {
            var responseDto = await _productService.GetUnAvailableProductsAsync();
            return Ok(new ApiResponse<IEnumerable<ProductResponseDTO>>(true,"Unavailable products retrieved successfully",responseDto));
        }

      
        [HttpGet("price-range")]
        public async Task<IActionResult> GetProductsByPriceRange([FromQuery] decimal minPrice, [FromQuery] decimal maxPrice)
        {
            var responseDto = await _productService.GetProductsByPriceRangeAsync(minPrice, maxPrice);

            return Ok(new ApiResponse<IEnumerable<ProductResponseDTO>>(
                true,"Products retrieved successfully",responseDto));

        }

        [HttpGet("top-selling")]
        public async Task<IActionResult> GetBestSellerProductsAsync()
        {
            var responseDTOs = await _productService.GetBestSellerProductsAsync();
            return Ok(new ApiResponse<IEnumerable<ProductResponseDTO>>(true, "Products best sellers successfully", responseDTOs));

        }


        [HttpGet("{productId}/recipes")]
        public async Task<IActionResult> GetRecipesByProductId([FromRoute] string productId)
        {
            var recipeListDTOs = await _productService.GetRecipeByProductIdAsync(productId);
            return Ok(new ApiResponse<IEnumerable<RecipeListDTO>>(true, "Recipes retrieved successfully", recipeListDTOs));
        }
    }
}
