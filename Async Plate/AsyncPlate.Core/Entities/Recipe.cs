using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Core.Entities
{
    public class Recipe
    {
        public double Quantity { get; set; } //quantity of inventory needed 



        public string ProductId { get; set; } = null!;
        public Product Product { get; set; } = null!;

        public string InventoryId { get; set; } = null!;
        public Inventory Inventory { get; set; } = null!;
    }
}
