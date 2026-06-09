using AsyncPlate.Application.DTOs.Offer;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Application.Validators.Offer
{
    public class AddOfferRequestValidator : AbstractValidator<AddOfferRequestDTO>
    {
        public AddOfferRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Offer name is required.")
                .MaximumLength(100).WithMessage("Offer name cannot exceed 100 characters.");

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Offer Title is required.")
                .MaximumLength(500).WithMessage("Offer Title cannot exceed 500 characters.");

            RuleFor(x => x.DiscountPercentage)
                .GreaterThan(0).WithMessage("Discount percentage must be greater than 0.")
                .LessThanOrEqualTo(100).WithMessage("Discount percentage cannot exceed 100.");

            RuleFor(x => x.StartDate)
                .GreaterThanOrEqualTo(DateTime.UtcNow).WithMessage("Start date cannot be in the past.")
                .LessThan(x => x.EndDate).WithMessage("Start date must be before end date.");
            
            RuleFor(x => x.CategoryIds).NotEmpty().WithMessage("At least one category must be associated with the offer.");
        }
    }
}
