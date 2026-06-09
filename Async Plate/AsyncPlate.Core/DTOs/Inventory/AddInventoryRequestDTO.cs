using AsyncPlate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Application.DTOs.Inventory
{
    public class AddInventoryRequestDTO
    {
        public string Name { get; set; } = string.Empty;
        public decimal CurrentStock { get; set; }
        public decimal MinStockLevel { get; set; }
        public string Unit { get; set; } = string.Empty;
        public decimal PurchasedUnitPrice { get; set; }// price i purchased
        
        public string Currency { get; set; } = string.Empty;
        public string SupplierId { get; set; } = string.Empty;


    }
}
