using AsyncPlate.Application.DTOs.Supplier;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Application.Validators.Supplier
{
    public class UpdateSupplierRequestValidator : AbstractValidator<UpdateSupplierRequestDTO>
    {
        public UpdateSupplierRequestValidator()
        {
            RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("Supplier name is required.")
            .MaximumLength(100);


            RuleFor(x => x.ContactPhone)
                .NotEmpty()
                .WithMessage("Contact phone is required.")
                .Matches(@"^\+?[0-9\s\-]{7,20}$")
                .WithMessage("Invalid phone number format.")
                .MaximumLength(20);

            RuleFor(x => x.Address)
                .NotEmpty()
                .WithMessage("Address is required.")
                .MaximumLength(50);

            RuleFor(x => x.City)
                .NotEmpty()
                .WithMessage("City is required.")
                .MaximumLength(50);
        }
    }
}
