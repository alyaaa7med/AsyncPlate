using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Domain.Entities
{
    public class Admin 
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();
        //public DateTime LastLoginDate { get; set; } 
        //public bool IsActive { get; set; } = true;

        public string AppUserId { get; set; } = null!;
        public AppUser AppUser { get; set; } = null!;
    }
}
