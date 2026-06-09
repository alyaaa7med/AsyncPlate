using AsyncPlate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Application.DTOs.Order
{

    public class OrderExtraItemRequestDTO
    {
        public int Quantity { get; set; }
        public decimal UnitPriceAtSale { get; set; }
        public string ProductId { get; set; } = string.Empty;
    }
}
