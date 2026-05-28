using AsyncPlate.Core.Common.DTOs;
using AsyncPlate.Core.DTOs.Inventory;
using AsyncPlate.Core.DTOs.Supplier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Core.Services.Interfaces
{
    public interface IInventoryService
    {

        //soft delete inventory

        //send low stock notification for admin and chef + trigger/cancel an automatic email to supplier 
        Task<InventoryResponseDTO> GetInventoryByIdAsync(string inventoryId);
        Task<InventoryResponseDTO> AddInventoryAsync(AddInventoryRequestDTO requestDTO);
        Task<InventoryResponseDTO> UpdateInventoryAsync(string inventoryId, UpdateInventoryRequestDTO requestDTO);
        Task<InventoryResponseDTO> DeleteInventoryAsync(string inventoryId);
        Task<PagedResult<InventoryResponseDTO>> GetAllInventoriesAsync(InventoryFilterDTO filterDto);
        Task<SupplierSummaryDTO> GetSupplierByInventoryIdAsync(string inventoryId); //no pagenation as 1 inventory -> 1 supplier
        Task<PagedResult<InventoryResponseDTO>> GetLowStockInventoriesAsync(InventoryFilterDTO filterDto);

    }
}      

