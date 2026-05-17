using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Core.Entities
{
    public  class OneTimeToken
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        //token
        //isvalid
        //expiry  --> if is still valid (one user call forget passowrd ) and not complete the process ,
        //the token need to be expired i need to expire it too

        public string Token { get; set; } = string.Empty;
        public DateTime ExpiryDate { get; set; } = DateTime.UtcNow.AddMinutes(20);//start when the object created in memory not when saved in DB
        public bool IsActive { get; set; } = true;

        public string AppUserId { get; set; } = null!;
        public AppUser AppUser { get; set; } = null!;


    }
}
