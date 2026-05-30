using AsyncPlate.Core.DTOs.Inventory;
using AsyncPlate.Core.DTOs.Product;
using AsyncPlate.Core.DTOs.Supplier;
using AsyncPlate.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Core.DTOs.Recipe
{
    public class RecipeResponseDTO
    {
        
        public double Quantity { get; set; } 

        public InventorySummaryDTO Inventory { get; set; } = new ();    

        public ProductSummaryDTO Product { get; set; } = new ();

    }
}
