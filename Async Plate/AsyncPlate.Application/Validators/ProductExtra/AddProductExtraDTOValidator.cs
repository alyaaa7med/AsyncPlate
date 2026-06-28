using AsyncPlate.Application.DTOs.ProductExtra;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Application.Validators.ExtraProduct
{

    public class AddProductExtraDTOValidator : AbstractValidator<AddProductExtraDTO>
    {
        public AddProductExtraDTOValidator()
        {

            RuleFor(x => x.ExtraProductIds)
                .NotNull()
                .WithMessage("Extras list is required.");

            RuleFor(x => x.ExtraProductIds)
                .NotEmpty()
                .WithMessage("At least one extra product must be provided.");

        }
    }
}
