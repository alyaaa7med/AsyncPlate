using AsyncPlate.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Core.DTOs.Order
{
    public class MakeOrderRequestDTO
    {
        public string Description { get; set; } = string.Empty;

        public string CustomerId { get; set; } = string.Empty;

        public List<OrderItemRequestDTO> OrderItems { get; set; } = new();
    }
}
