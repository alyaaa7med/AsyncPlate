using AsyncPlate.Application.DTOs.Product;
using AsyncPlate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Application.DTOs.Order
{
    public class OrderItemResponseDTO
    {

        public string Id { get; set; } = Guid.NewGuid().ToString();
        public int Quantity { get; set; } // of product 
        public decimal UnitPriceAtSale { get; set; }

        public string OrderId { get; set; } = null!;
        public string ProductId { get; set; } = null!;

        public IEnumerable<OrderExtraItemResponseDTO> Extras { get; set; } = new List<OrderExtraItemResponseDTO>();
    }
}
