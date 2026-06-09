using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Application.DTOs.Order
{
    public class OrderItemRequestDTO
    {

        public int Quantity { get; set; }
        public decimal UnitPriceAtSale { get; set; }
        public string ProductId { get; set; } = string.Empty;


        //we need these id for checking if these extra products with the main product are valid
        public List<OrderExtraItemRequestDTO> ExtraItems { get; set; } = new();//order items 
    

    }


   
}
