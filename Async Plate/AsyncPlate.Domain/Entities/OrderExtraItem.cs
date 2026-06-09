using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Domain.Entities
{
    public class OrderExtraItem //orderitem : orderextraitem 1:M 
    {
        //order extra item to know extra items choosen by the userrrr 
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public int Quantity { get; set; }
        public decimal UnitPriceAtSale { get; set; }

        public string OrderItemId { get; set; } = null!;
        public OrderItem OrderItem { get; set; } = null!;

        public string ProductId { get; set; } = null!;
        public Product Product { get; set; } = null!;



    }
}
