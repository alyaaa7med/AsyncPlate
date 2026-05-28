using AsyncPlate.API.Models;
using AsyncPlate.Core;
using AsyncPlate.Core.Common.DTOs;
using AsyncPlate.Core.DTOs.Authentication;
using AsyncPlate.Core.DTOs.Inventory;
using AsyncPlate.Core.DTOs.Supplier;
using AsyncPlate.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AsyncPlate.API.Controllers
{
    [ApiController]
    [Route("api/[controller]s")]
    //[Authorize(Roles ="Admin")]
    public class SupplierController : ControllerBase
    {
        private readonly ISupplierService _supplierService;

        public SupplierController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }

        [HttpPost()]
        public async Task<IActionResult> AddSupplier([FromBody] AddSupplierRequestDTO requestDTO)
        {
            var responseDTO = await _supplierService.AddSupplierAsync(requestDTO);
            return Created($"/suppliers/{responseDTO.Id}", new ApiResponse<SupplierResponseDTO>(true, "Supplier added successfully", responseDTO));

        }
        [HttpGet("{supplierId}")]
        public async Task<IActionResult> GetSupplierById(string supplierId)
        {
            var responseDTO = await _supplierService.GetSupplierByIdAsync(supplierId);
            return Ok(new ApiResponse<SupplierResponseDTO>(true, "Supplier retrieved successfully", responseDTO));
        }

        [HttpGet()]
        public async Task<IActionResult> GetAllSuppliers([FromQuery] SupplierFilterDTO filterDto)
        {
            var responseDTO = await _supplierService.GetAllSuppliersAsync(filterDto);
            return Ok(new ApiResponse<PagedResult<SupplierResponseDTO>>(true, "Suppliers retrieved successfully", responseDTO));
        }

        [HttpPut("{supplierId}")]
        public async Task<IActionResult> UpdateSupplier([FromRoute] string supplierId, [FromBody] UpdateSupplierRequestDTO requestDTO)
        {

            var responseDTO = await _supplierService.UpdateSupplierAsync(supplierId, requestDTO);
            return Ok(new ApiResponse<SupplierResponseDTO>(true, "Supplier updated successfully", responseDTO));
        }
        [HttpDelete("{supplierId}")]
        public async Task<IActionResult> DeleteSupplier(string supplierId)
        {
            var responseDTO = await _supplierService.DeleteSupplierAsync(supplierId);
            return Ok(new ApiResponse<SupplierResponseDTO>(true, "Supplier deleted successfully", responseDTO));
        }

        [HttpGet("{supplierId}/inventories")]

        public async Task<IActionResult> GetInventoriesBySupplierId([FromRoute] string supplierId, [FromQuery] InventoryFilterDTO filterDto)
        {
            var responseDTO = await _supplierService.GetAllInventoriesBySupplierIdAsync(supplierId,filterDto);
            return Ok(new ApiResponse<PagedResult<InventorySummaryDTO>>(true, "Inventories retrieved successfully", responseDTO));
        }
    }
}
        

