using AsyncPlate.Application.DTOs.Product;
using AsyncPlate.Application.DTOs.Supplier;
using AsyncPlate.Application.DTOs.Inventory;
using AsyncPlate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Application.DTOs.Recipe
{
    public class RecipeResponseDTO
    {
        
        public double Quantity { get; set; } 

        public InventorySummaryDTO Inventory { get; set; } = new ();    

        public ProductSummaryDTO Product { get; set; } = new ();

    }
}
