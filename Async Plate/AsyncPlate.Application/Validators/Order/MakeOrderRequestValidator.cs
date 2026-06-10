using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsyncPlate.Application.DTOs.Order;
using AsyncPlate.Application.Validators.Order;
using FluentValidation;
namespace AsyncPlate.Application.Validators.Order
{
    public class MakeOrderRequestValidator : AbstractValidator<MakeOrderRequestDTO>
    {
        public MakeOrderRequestValidator()
        {

            //RuleFor(x => x.CustomerId)
            //    .NotEmpty().WithMessage("CustomerId is required.");

            RuleFor(x => x.Description)
                .NotEmpty().WithMessage("Description is required.");
            RuleFor(x => x.OrderItems)
                .NotEmpty()
                .WithMessage("Order must contain at least one item.");

            RuleForEach(x => x.OrderItems)
                .SetValidator(new OrderItemRequestValidator());
        }
    }
}
