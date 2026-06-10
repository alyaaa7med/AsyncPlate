using AsyncPlate.Application.DTOs.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Application.DTOs.Admin
{
    public class CreateAdminRequestDTO
    {
        public SignupAppUserRequestDTO AppUser { get; set; } = new();

    }
}
