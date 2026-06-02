using AsyncPlate.Core.DTOs.Order;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Core.Validators.Order
{
    public class OrderExtraItemRequestValidator : AbstractValidator<OrderExtraItemRequestDTO>
    {
        public OrderExtraItemRequestValidator()
        {
            RuleFor(x => x.Quantity).NotEmpty()
                .GreaterThan(0).WithMessage("Quantity must be greater than 0.");

            RuleFor(x => x.UnitPriceAtSale).NotEmpty()
                .GreaterThan(0).WithMessage("Unit price at sale must be greater than 0.");

            RuleFor(x => x.ProductId)
                .NotEmpty().WithMessage("Product ID is required.");
        }
    }
}

