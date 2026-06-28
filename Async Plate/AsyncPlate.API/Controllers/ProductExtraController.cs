using AsyncPlate.API.Models;
using AsyncPlate.Application.DTOs.Product;
using AsyncPlate.Application.DTOs.ProductExtra;
using AsyncPlate.Application.Services.Implementation;
using AsyncPlate.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AsyncPlate.API.Controllers
{

    [ApiController]
    [Route("api/Products")]
    [Authorize(Roles = "Admin")]
    public class ProductExtraController : ControllerBase
    {
        private readonly IProductExtraService _productExtraService;

        public ProductExtraController(IProductExtraService productExtraService)
        {
            _productExtraService = productExtraService;
        }


        [HttpPost("{productId}/Extras")]
        public async Task<IActionResult> AddExtrasProducts([FromBody] AddProductExtraDTO productextraRequestDTO, [FromRoute] string productId)
        {
            var responseDto = await _productExtraService.AddExtrasAsync(productId,productextraRequestDTO);
            return Created($"/productextras/{responseDto.Id}", new ApiResponse<ProductWithExtrasDTO>(true, "Product extras added successfully", responseDto));
        }

        [HttpPut("{productId}/Extras")]
        public async Task<IActionResult> UpdateExtrasProducts([FromRoute] string productId,[FromBody] UpdateProductExtrasDTO productExtraRequestDTO)
        {
            var responseDto = await _productExtraService.UpdateExtrasAsync(productId, productExtraRequestDTO);

            return Ok(new ApiResponse<ProductWithExtrasDTO>( true,"Product extras updated successfully",responseDto));
        }

        [HttpDelete("{productId}/Extras/{extraProductId}")]
        public async Task<IActionResult> DeleteExtraProduct([FromRoute] string productId, [FromRoute] string extraProductId)
        {
            await _productExtraService.DeleteExtraAsync(productId, extraProductId);

            return Ok(new ApiResponse<object>( true,"Product extra removed successfully",null));
        }

        [HttpDelete("{productId}/Extras")]
        public async Task<IActionResult> RemoveAllExtras( [FromRoute] string productId)
        {
            await _productExtraService.DeleteAllExtrasAsync(productId);

            return Ok(new ApiResponse<object>(true,"All product extras removed successfully",null));
        }

        [HttpGet("{productId}/Extras")]
        public async Task<IActionResult> GetProductExtras([FromRoute] string productId)
        {
            var responseDto = await _productExtraService.GetExtrasByProductIdAsync(productId);

            return Ok(new ApiResponse<IEnumerable<ProductExtraDTO>>(true,"Product extras retrieved successfully",responseDto));
        }
    }
}
