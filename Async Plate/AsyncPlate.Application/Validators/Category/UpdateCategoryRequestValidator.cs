using AsyncPlate.Application.DTOs.Category;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace AsyncPlate.Application.Validators.Category
{

    public class UpdateCategoryRequestValidator : AbstractValidator<UpdateCategoryRequestDTO>
    {
        public UpdateCategoryRequestValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Description)
                .MaximumLength(500);

            RuleFor(x => x.ImageUrl)
                .Must(BeAValidImage)
                .When(x => x.ImageUrl != null)
                .WithMessage("Only image files (.jpg, .jpeg, .png, .webp) are allowed.");
        }



        private static bool BeAValidImage(IFormFile? file)
        {
            if (file == null)
                return true;

            var allowedExtensions = new[]
            {".jpg",".jpeg",".png",".webp"};
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

            return allowedExtensions.Contains(extension);
        }

    
    }
}