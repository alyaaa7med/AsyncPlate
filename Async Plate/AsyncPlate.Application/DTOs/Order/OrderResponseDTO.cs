using AsyncPlate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Application.DTOs.Order
{
    public class OrderResponseDTO
    {
        public string Id { get; set; } = string.Empty;
        public DateTime OrderDate { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty; //should be realtime in getall orders for chef/admin
        public decimal TotalAmountPrice { get; set; }
        public decimal TotalFee { get; set; }
        public decimal TotalFeeTotal { get; set; }

        public string CustomerId { get; set; } = string.Empty;
        public string kitchenChefId { get; set; } = string.Empty;


        public IEnumerable<OrderItemResponseDTO> OrderItems { get; set; } = new List<OrderItemResponseDTO>();


    }
}
