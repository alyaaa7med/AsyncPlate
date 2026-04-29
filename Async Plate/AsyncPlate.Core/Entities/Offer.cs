using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Core.Entities
{
    public class Offer //per category 
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string Title { get; set; } = string.Empty;   //zero length string & This prevents the
                                                            //dreaded NullReferenceException
                                                            //if you try to read  before assigning it
        public decimal DiscountPercentage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public bool IsActive { get; set; }

        public ICollection<Category> Categories { get; set; } = new List<Category>();
    }
}
