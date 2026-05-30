using AsyncPlate.API.Models;
using AsyncPlate.Core.DTOs.Category;
using AsyncPlate.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AsyncPlate.API.Controllers
{

    [ApiController]
    [Route("api/[controller]s")]
    //[Authorize(Roles = "Admin")]
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
    }
}
