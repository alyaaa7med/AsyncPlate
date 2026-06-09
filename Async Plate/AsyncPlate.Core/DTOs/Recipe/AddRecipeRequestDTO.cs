using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Application.DTOs.Recipe
{
    public class AddRecipeRequestDTO
    {
        public decimal Quantity { get; set; } //quantity of inventory needed 
        public string Unit { get; set; } = string.Empty; //unit of measurement for the quantity (e.g., grams, liters)
        public string ProductId { get; set; } = string.Empty;
        public string InventoryId { get; set; } = string.Empty;

    }
}
