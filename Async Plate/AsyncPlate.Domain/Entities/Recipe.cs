using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Domain.Entities
{
    public class Recipe
    {
        public decimal Quantity { get; set; } //quantity of inventory needed 

        public string Unit { get; set; } = null!; //unit of measurement for the quantity (e.g., grams, liters)
        public string ProductId { get; set; } = null!;
        public Product Product { get; set; } = null!;

        public string InventoryId { get; set; } = null!;
        public Inventory Inventory { get; set; } = null!;
    }
}
