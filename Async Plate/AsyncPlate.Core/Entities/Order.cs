using AsyncPlate.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Core.Entities
{
    public class Order
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public string Description { get; set; } = string.Empty;
        public decimal TotalAmountPrice { get; set; }//e.g. 
        public string Status { get; set; } = "Pending"; // e.g., Pending, Completed, Cancelled
        public decimal TotalFee { get; set; }
        public decimal TotalFeeTotal { get; set; }

        public Review? Review { get; set; } 
        public Payment? Payment { get; set; }

        public string? CustomerId { get; set; } 
        public Customer? Customer { get; set; }

        public string? KitchenChefId { get; set; }
        public KitchenChef? KitchenChef { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
}
