using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Core.Entities
{
    public  class ProductExtra
    {
        public string ProductId { get; set; } = null!;        // Main product
        public Product Product { get; set; } = null!;

        public string ExtraProductId { get; set; } = null!;   // Extra product
        public Product ExtraProduct { get; set; } = null!;

    }
}
