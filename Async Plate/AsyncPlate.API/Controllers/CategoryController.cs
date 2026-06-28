using AsyncPlate.API.Models;
using AsyncPlate.Application.Common.DTOs;
using AsyncPlate.Application.DTOs.Category;
using AsyncPlate.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AsyncPlate.API.Controllers
{

    [ApiController]
    [Route("api/Categories")]
    [Authorize(Roles = "Admin")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory([FromForm] AddCategoryRequestDTO categoryRequestDTO)
        {
            var responseDto = await _categoryService.AddCategoryAsync(categoryRequestDTO);
            return Created($"/categories/{responseDto.Id}", new ApiResponse<CategoryResponseDTO>(true, "Category created successfully", responseDto));

        }

        [HttpGet("{categoryId}")]
        public async Task<IActionResult> GetCategoryById([FromRoute]string categoryId)
        {
            var responseDto = await _categoryService.GetCategoryByIdAsync(categoryId);
            return Ok(new ApiResponse<CategoryResponseDTO>(true, "Category retrieved successfully", responseDto));
        }


        [HttpGet]
        public async Task<IActionResult> GetAllCategories([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var responseDto = await _categoryService.GetAllAsync(pageNumber, pageSize);

            return Ok(new ApiResponse<PagedResult<CategoryResponseDTO>>(true,"Categories retrieved successfully",responseDto));
        }

        
        [HttpPut("{categoryId}")]
        public async Task<IActionResult> UpdateCategory(string categoryId,[FromForm] UpdateCategoryRequestDTO requestDTO)
        {
            var responseDto = await _categoryService.UpdateCategoryAsync(categoryId, requestDTO);

            return Ok(new ApiResponse<CategoryResponseDTO>(true,"Category updated successfully",responseDto));
        }

        [HttpDelete("{categoryId}")]
        public async Task<IActionResult> DeleteCategory([FromRoute] string categoryId)
        {
            await _categoryService.DeleteCategoryAsync(categoryId);

            return Ok(new ApiResponse<CategoryResponseDTO>(true,"Category deleted successfully",null));
        }
    }
}
