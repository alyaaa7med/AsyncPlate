using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Core.Entities
{
    public class Review //for the order not a product
    {
        public string Id { get; set; }= Guid.NewGuid().ToString();
        public int Rating { get; set; } //1 to 5
        public string Message { get; set; } = string.Empty;


        public string OrderId { get; set; }= null!;
        public Order Order { get; set; } = null!;

    }
}
