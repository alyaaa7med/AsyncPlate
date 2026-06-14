using AsyncPlate.Application.DTOs.Inventory;
using AsyncPlate.Application.DTOs.Product;
using AsyncPlate.Application.DTOs.Supplier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Application.DTOs
{
    public class DailyReportDTO
    {
        public int TotalOrders { get; set; }
        public int CompletedOrders { get; set; }
        public int CancelledOrders { get; set; }

        public decimal TotalRevenue { get; set; }


        public List<ProductResponseDTO> TopProducts { get; set; } = new List<ProductResponseDTO>();
        public List<InventoryResponseDTO> LowStockItems { get; set; } = new List<InventoryResponseDTO>();
    }

}
