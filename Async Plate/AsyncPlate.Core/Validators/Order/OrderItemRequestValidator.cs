using AsyncPlate.Core.DTOs.Order;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Core.Validators.Order
{
    public class OrderItemRequestValidator : AbstractValidator<OrderItemRequestDTO>
    {
        public OrderItemRequestValidator()
        {
            RuleFor(i => i.Quantity)
           .NotEmpty().GreaterThan(0)
           .WithMessage("Quantity must be greater than zero.");

            RuleFor(i => i.ProductId)
                .NotEmpty()
                .WithMessage("ProductId is required.");

            RuleForEach(x => x.ExtraItems)
                 .SetValidator(new OrderExtraItemRequestValidator()); //no need to check in service 
        }
    }
}
