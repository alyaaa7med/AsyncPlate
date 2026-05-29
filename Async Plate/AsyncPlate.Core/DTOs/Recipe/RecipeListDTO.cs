using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Core.DTOs.Recipe
{
    public class RecipeListDTO
    {
        public string InventoryName { get; set; }= string.Empty;
        public decimal Quantity { get; set; }
    }
}
