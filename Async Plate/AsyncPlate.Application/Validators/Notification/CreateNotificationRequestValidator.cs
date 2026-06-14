using AsyncPlate.Application.DTOs.Notification;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Application.Validators.Notification
{
    public class CreateNotificationRequestValidator : AbstractValidator<CreateNotificationRequestDTO>
    {
        public CreateNotificationRequestValidator()
        {

            RuleFor(n => n.Title)
                      .NotEmpty()
                      .WithMessage("Title  is required.")
                      .MaximumLength(70);

            RuleFor(n => n.Message)
                .NotEmpty()
                .MaximumLength(70)
                .WithMessage("Message is required.");


            RuleFor(n => n.Url)
                .NotEmpty()
                .WithMessage("Message is required.");

        }


    }
}
