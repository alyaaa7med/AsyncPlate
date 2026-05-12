using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Core.DTOs.Authentication
{
    public class SignupCustomerResponseDTO
    {

        //should be the same as customer entity 
        public string Id { get; set; } = string.Empty;
        public SignupAppUserResponseDTO AppUser { get; set; } = new();
        public int LoyaltyPoints { get; set; }


    }
}
