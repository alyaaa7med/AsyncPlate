using AsyncPlate.Core.DTOs.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Core.DTOs.Product
{
    public class ProductResponseDTO
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; }= string.Empty;
        public decimal BasePrice { get; set; } // price for selling 
        public bool IsAvailable { get; set; }
        public int TotalTimesOrdered { get; set; } //to calculate best sellers
        
        public CategorySummaryDTO CategorySummary { get; set; } = new();

    }
}
