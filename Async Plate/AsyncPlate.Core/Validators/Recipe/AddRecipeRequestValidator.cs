using AsyncPlate.Application.DTOs.Recipe;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Application.Validators.Recipe
{
    public class AddRecipeRequestValidator : AbstractValidator<AddRecipeRequestDTO>
    {
        public AddRecipeRequestValidator()
        {
            RuleFor(x => x.Quantity)
                .GreaterThan(0).WithMessage("Quantity must be greater than 0.");
            RuleFor(x => x.ProductId)
                .NotEmpty().WithMessage("ProductId is required.");
            RuleFor(x => x.InventoryId)
                .NotEmpty().WithMessage("InventoryId is required");
        }
    }
}
