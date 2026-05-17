using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Core.Entities
{
    public class Payment
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string? OrderId { get; set; }
        public Order? Order { get; set; } 

    }
}
