using AsyncPlate.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Application.Interfaces.Repositories
{
    public interface IInventoryRepo : IBaseRepo<Inventory>
    {
        Task<bool> AnyInventoryAsync(string name);
        IQueryable<Inventory> FilterByName(string name);

        IQueryable<Inventory> GetAllWithSuppliers();
        IQueryable<Inventory> GetInventoriesBySupplierId(string supplierId);
        Task<Inventory?> GetInventoryWithSupplierAsync(string inventoryId);
        IQueryable<Inventory> GetLowStockInventory();
        Task<List<Inventory>> GetInventoriesByIdsAsync(List<string> inventoryIds);

    }
}
