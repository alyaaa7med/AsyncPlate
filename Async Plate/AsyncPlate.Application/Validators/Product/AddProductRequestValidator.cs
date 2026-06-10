using AsyncPlate.Application.DTOs.Product;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Application.Validators.Product
{
    public class AddProductRequestValidator : AbstractValidator<AddProductRequestDTO>
    {
        public AddProductRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
            RuleFor(x => x.Type).NotEmpty().WithMessage("Type is required.");
            RuleFor(x => x.BasePrice).GreaterThan(0).WithMessage("Base price must be greater than zero.");
            RuleFor(x => x.CategoryId).NotEmpty().WithMessage("Category ID is required.");
        }
    }
}
