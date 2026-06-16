using AsyncPlate.Application.Interfaces.Repositories;
using AsyncPlate.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Infrastructure.Data.Repositories
{
    public class InventoryRepo : GenericRepo<Inventory>, IInventoryRepo
    {
        public InventoryRepo(AppDbContext context) : base(context)
        {
        }
        public async Task<bool> AnyInventoryAsync(string name)
        {
            return await _context.Inventories.AnyAsync(i => i.Name== name);
        }

        public IQueryable<Inventory> FilterByName(string name)
        {
            return _context.Inventories.Where(x => x.Name.Contains(name));
        }

      
        public IQueryable<Inventory> GetAllWithSuppliers()
        {
            //to be eager loadeddd 
            return _context.Inventories.Include(i => i.Supplier);
        }
        public async Task<Inventory?> GetInventoryWithSupplierAsync(string inventoryId)
        {
            return await _context.Inventories.Include(i => i.Supplier).SingleOrDefaultAsync(i => i.Id == inventoryId);
        }

        public IQueryable<Inventory> GetLowStockInventory()
        {
            return _context.Inventories.Where(i => i.CurrentStock < i.MinStockLevel).Include(i=>i.Supplier);
        }

        public IQueryable<Inventory> GetInventoriesBySupplierId(string supplierId)
        {
            return _context.Inventories.Where(i => i.SupplierId == supplierId).Include(i => i.Supplier);
        }
        public  async Task<List<Inventory>> GetInventoriesByIdsAsync(List<string> inventoryIds)
        {
            return await _context.Inventories.Where(i => inventoryIds.Contains(i.Id)).ToListAsync();
        }

        public async Task<List<Inventory>> GetLowStockWithSuppliersAsync()
        {

            return await _context.Inventories .Where(i => i.CurrentStock < i.MinStockLevel)
                        .Include(i => i.Supplier).ToListAsync();
        }
    }
}
