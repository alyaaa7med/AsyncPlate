using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Core.Entities
{
    public class KitchenChef  
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string AppUserId { get; set; } = null!;
        public AppUser AppUser { get; set; } = null!;//must in the db mapping 

        public ICollection<Order> Orders { get; set; } = new List<Order>();

    }
   
}
