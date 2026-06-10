using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Application.DTOs.Authentication
{
    public class ForgetPasswordRequestDTO
    {
        public string Email { get; set; } = string.Empty;
    }
}
