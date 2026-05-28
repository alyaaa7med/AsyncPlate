using AsyncPlate.Core.DTOs.Admin;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Core.Validators.Admin
{
    public class SignupAdminRequestValidator : AbstractValidator< CreateAdminRequestDTO>
    {
        public SignupAdminRequestValidator()
        {
            
        }
    }
}
