using AsyncPlate.Application.DTOs.Review;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Application.Validators.Review
{
    public class UpdateReviewRequestValidator : AbstractValidator<UpdateReviewRequestDTO>
    {
        public UpdateReviewRequestValidator()
        {
            RuleFor(x => x.Rating)
                .InclusiveBetween(1, 5);

            RuleFor(x => x.Message)
                .NotEmpty()
                .MaximumLength(500);
        }
    }


}
