using AsyncPlate.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Core.DTOs.Inventory
{
    public class AddInventoryRequestDTO
    {
        public string Name { get; set; } = string.Empty;
        public decimal CurrentStock { get; set; }
        public decimal MinStockLevel { get; set; }
        public string Unit { get; set; } = string.Empty;
        public decimal PurchasedUnitPrice { get; set; }// price i purchased
        public string SupplierId { get; set; } = string.Empty;


    }
}
