using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Core.DTOs.Product
{
    public class AddProductRequestDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public decimal BasePrice { get; set; } 
        //public bool IsAvailable { get; set; }
        public string CategoryId { get; set; } = string.Empty;
    }
}
