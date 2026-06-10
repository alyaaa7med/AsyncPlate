using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Application.DTOs.Authentication
{
    public class SignupKitchenChefRequestDTO
    {
        public SignupAppUserRequestDTO AppUser { get; set; } = new();
        
    }
}

