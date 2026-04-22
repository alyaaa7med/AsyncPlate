using FluentValidation;
using AsyncPlate.Core.DTOs.Authentication;

namespace AsyncPlate.Core.Validators.Authentication
{
    public class SignupGuestRequestValidator : AbstractValidator<SignupGuestRequestDTO>
    {
        public SignupGuestRequestValidator()
        {
            // Name Validations
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required.")
                .MaximumLength(50).WithMessage("First name cannot exceed 50 characters.")
                .Matches(@"^[a-zA-Z ]+$").WithMessage("First name can only contain letters.");

            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required.")
                .MaximumLength(50).WithMessage("Last name cannot exceed 50 characters.")
                .Matches(@"^[a-zA-Z ]+$").WithMessage("Last name can only contain letters.");

            // Contact Validations
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("A valid email address is required.");

            RuleFor(x => x.PhoneNumber)
                .NotEmpty().WithMessage("Phone number is required.")
                .Matches(@"^01[0125][0-9]{8}$").WithMessage("Please enter a valid Egyptian phone number.");

            RuleFor(x => x.Address)
                .NotEmpty().WithMessage("Address is required.")
                .MinimumLength(10).WithMessage("Please provide a more detailed address.");

            // Password Complexity
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required.")
                .MinimumLength(8).WithMessage("Password must be at least 8 characters long.")
                .Matches(@"[A-Z]").WithMessage("Password must contain at least one uppercase letter.")
                .Matches(@"[a-z]").WithMessage("Password must contain at least one lowercase letter.")
                .Matches(@"[0-9]").WithMessage("Password must contain at least one digit.")
                .Matches(@"[\!\?\*\.]").WithMessage("Password must contain at least one special character (!?*.).");

            // Password Confirmation
            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("Please confirm your password.")
                .Equal(x => x.Password).WithMessage("Passwords do not match.");


        }
    }
}
