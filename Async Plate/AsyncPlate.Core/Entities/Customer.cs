using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Core.Entities
{
    public class Customer 
    {

        public string Id { get; set; } = Guid.NewGuid().ToString();
        public int LoyaltyPoints { get; set; }


        public string AppUserId { get; set; } = null!;
        public AppUser AppUser { get; set; } = null!;//must in the db mapping

        public ICollection<Order> Orders { get; set; } = new List<Order>();

        public ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    }
}
