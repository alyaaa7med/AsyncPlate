using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Application.DTOs.Recipe
{
    public class RecipeFilterDTO
    {
        //filtering

        public string? ProductName { get; set; }
        public string? InventoryName { get; set; }


        //pagination
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

    }
}
