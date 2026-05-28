using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Core.DTOs.Inventory
{
    public class UpdateInventoryRequestDTO
    {
        public string Name { get; set; } = string.Empty;
        public decimal CurrentStock { get; set; }
        public decimal MinStockLevel { get; set; }
        public string Unit { get; set; } = string.Empty;
        public decimal PurchasedUnitPrice { get; set; }
        public string SupplierId { get; set; } = string.Empty;
    }
}
