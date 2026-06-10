using AsyncPlate.Application.DTOs.Offer;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Application.Validators.Offer
{
    public class UpdateOfferRequestValidator :AbstractValidator<UpdateOfferRequestDTO>
    {
        public UpdateOfferRequestValidator()
        {
            RuleFor(o => o.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(100).WithMessage("Title cannot exceed 100 characters.");


            RuleFor(o => o.DiscountPercentage)
                .GreaterThan(0).WithMessage("Discount percentage must be greater than 0.")
                .LessThanOrEqualTo(100).WithMessage("Discount percentage cannot exceed 100.");

            RuleFor(o => o.StartDate)
                .GreaterThanOrEqualTo(DateTime.UtcNow).WithMessage("Start date cannot be in the past.")
                .LessThanOrEqualTo(o => o.EndDate).WithMessage("Start date must be before or equal to end date.");

            RuleFor(o => o.EndDate)
                .GreaterThanOrEqualTo(o => o.StartDate).WithMessage("End date must be after or equal to start date.");
        
            RuleFor(o=>o.CategoryIds)
                .NotEmpty().WithMessage("At least one category must be associated with the offer.");
        }
    }
}

