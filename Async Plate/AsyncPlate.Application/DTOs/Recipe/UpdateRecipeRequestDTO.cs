using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Application.DTOs.Recipe
{
    public class UpdateRecipeRequestDTO
    {
        public double Quantity { get; set; }
        public string Unit { get; set; } = string.Empty;
        public string ProductId { get; set; } = string.Empty;
        public string InventoryId { get; set; } = string.Empty;
    }
}
