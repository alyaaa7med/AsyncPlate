using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Core.DTOs.Inventory
{
    public class InventoryResponseDTO
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public decimal CurrentStock { get; set; }
        public decimal MinStockLevel { get; set; }
        public string Unit { get; set; } = string.Empty;
        public decimal PurchasedUnitPrice { get; set; }
        public SupplierSummaryDTO Supplier { get; set; } = new();
    }
}
