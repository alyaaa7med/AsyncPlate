using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Application.DTOs.ProductExtra
{
    public class ProductWithExtrasDTO
    {
            public string Id { get; set; } =string.Empty;

            public string Name { get; set; } = string.Empty;

            public decimal BasePrice { get; set; }

            public List<ProductExtraDTO> Extras { get; set; } = new();
        }
    
}
