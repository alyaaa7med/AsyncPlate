using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Core.Entities
{
    public class Category
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; }=string.Empty;

        public string ImageUrl { get; set; } = string.Empty;
        
        public string? OfferId { get; set; }
        public Offer? CurrentOffer { get; set; }

        public ICollection<Product> Products { get; set; } = new List<Product>();


    }
}
