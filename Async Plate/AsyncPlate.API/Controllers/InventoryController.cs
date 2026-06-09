using AsyncPlate.API.Models;
using AsyncPlate.Application.Common.DTOs;
using AsyncPlate.Application.DTOs.Inventory;
using AsyncPlate.Application.Services.Interfaces;
using AsyncPlate.Application.DTOs.Supplier;
using AsyncPlate.Application.Services.Implementation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AsyncPlate.API.Controllers
{
    [ApiController]
    [Route("api/Inventories")]
    //[Authorize(Roles = "Admin")]
    public class InventoryController : ControllerBase
    {
        private readonly IInventoryService _inventoryService;

        public InventoryController(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }
        [HttpPost()]
        public async Task<IActionResult> AddInventory([FromBody] AddInventoryRequestDTO requestDTO)
        {
            var responseDTO = await _inventoryService.AddInventoryAsync(requestDTO);
            return Created($"/inventories/{responseDTO.Id}", new ApiResponse<InventoryResponseDTO>(true, "Inventory added successfully", responseDTO));

        }
        [HttpGet("{inventoryId}")]
        public async Task<IActionResult> GetInventoryById([FromRoute]string inventoryId)
        {
            var responseDTO = await _inventoryService.GetInventoryByIdAsync(inventoryId);
            return Ok(new ApiResponse<InventoryResponseDTO>(true, "Inventory retrieved successfully", responseDTO));
        }

        [HttpGet()]
        public async Task<IActionResult> GetAllInventories([FromQuery] InventoryFilterDTO filterDto)
        {
            var responseDTO = await _inventoryService.GetAllInventoriesAsync(filterDto);
            return Ok(new ApiResponse<PagedResult<InventoryResponseDTO>>(true, "Inventories retrieved successfully", responseDTO));
        }

        [HttpPut("{inventoryId}")]
        public async Task<IActionResult> UpdateInventory([FromRoute] string inventoryId, [FromBody] UpdateInventoryRequestDTO requestDTO)
        {

            var responseDTO = await _inventoryService.UpdateInventoryAsync(inventoryId, requestDTO);
            return Ok(new ApiResponse<InventoryResponseDTO>(true, "Inventory updated successfully", responseDTO));
        }
        [HttpDelete("{inventoryId}")]
        public async Task<IActionResult> DeleteInventory([FromRoute]string inventoryId)
        {
            var responseDTO = await _inventoryService.DeleteInventoryAsync(inventoryId);
            return Ok(new ApiResponse<InventoryResponseDTO>(true, "Inventory deleted successfully", responseDTO));
        }

        [HttpGet("{inventoryId}/supplier")]
        public async Task<IActionResult> GetSupplier([FromRoute] string inventoryId)
        {
            var responseDTO = await _inventoryService.GetSupplierByInventoryIdAsync(inventoryId);
            return Ok(new ApiResponse<SupplierSummaryDTO>(true, "Supplier retrieved successfully", responseDTO));
        }
        [HttpGet("low-stock")]
        public async Task<IActionResult> GetLowStockInventories([FromQuery] InventoryFilterDTO filterDto)
        {
            var responseDTO = await _inventoryService.GetLowStockInventoriesAsync(filterDto);
            return Ok(new ApiResponse<PagedResult<InventoryResponseDTO>>(true, "Low stock inventories retrieved successfully", responseDTO));
        }
    }
}
