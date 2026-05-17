using AsyncPlate.Core.DTOs.Authentication;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Core.Validators.Authentication
{
    public class ForgetPasswordRequestValidator : AbstractValidator<ForgetPasswordRequestDTO>
    {
        public ForgetPasswordRequestValidator()
        {
            RuleFor(x => x.Email)
                           .NotEmpty().WithMessage("Email is required.")
                           .EmailAddress().WithMessage("A valid email address is required.");

        }
    }
}
