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


        public string AppUserId { get; set; } = string.Empty;//zero length string & This prevents the
                                                             //dreaded NullReferenceException
                                                             //if you try to read  before assigning it
        public AppUser AppUser { get; set; } = null!;//must in the db mapping

        public ICollection<Order> Orders { get; set; } = new List<Order>();


    }
}
