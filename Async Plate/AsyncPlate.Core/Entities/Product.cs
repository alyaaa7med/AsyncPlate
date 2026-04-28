using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Core.Entities
{
    public class Product
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Name { get; set; } = string.Empty;
        public Type Type { get; set; }
        public decimal BasePrice { get; set; } // price for selling 
        public bool IsAvailable { get; set; }
        public int TotalTimesOrdered { get; set; } //to calculate best sellers


        public string CategoryId { get; set; } = string.Empty;
        public Category Category { get; set; } = null!;
        public ICollection<Recipe> Recipes { get; set; } = new List<Recipe>();
        public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();


        // 1. The list of connections where THIS product is the MAIN product
        public ICollection<ProductExtra> MainProducts { get; set; } = new List<ProductExtra>();

        // 2. The list of connections where THIS product is the EXTRA product
        public ICollection<ProductExtra> ExtraProducts { get; set; } = new List<ProductExtra>();


    }

    public enum Type
    {
        Main,
        Extra
    }
}
