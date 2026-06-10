using FluentValidation;
using AsyncPlate.Application.DTOs.Authentication;

namespace AsyncPlate.Application.Validators.Authentication
{
    public class SignupCustomerRequestValidator : AbstractValidator<SignupCustomerRequestDTO>
    {
        public SignupCustomerRequestValidator()
        {
           

        }
    }
}
