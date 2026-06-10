using AsyncPlate.Application.DTOs.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Application.DTOs.Admin
{
    public class AdminResponseDTO
    {
        public string Id { get; set; } = string.Empty;
        public SignupAppUserResponseDTO AppUser { get; set; } = new();
    }
}
