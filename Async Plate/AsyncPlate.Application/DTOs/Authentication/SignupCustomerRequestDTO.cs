using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Application.DTOs.Authentication
{
    public class SignupCustomerRequestDTO
    {
        //Should be the same name  as AppUser  entity and customer entity or i need to define it in 
        //the mapping profile

        public SignupAppUserRequestDTO AppUser { get; set; } = new();
     
    }
}
  
