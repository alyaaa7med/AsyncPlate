using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Application.DTOs.Order
{
    public class OrderExtraItemResponseDTO
    {
        public string Id { get; set; } = string.Empty;
        public int Quantity { get; set; } 
        public decimal UnitPriceAtSale { get; set; }
        public string OrderItemId { get; set; } = string.Empty;
        public string ProductId { get; set; } = string.Empty;
    }
}
