using AsyncPlate.Application.DTOs.Category;
using AsyncPlate.Application.Interfaces;
using AsyncPlate.Application.Interfaces.Services;
using AsyncPlate.Application.Services.Interfaces;
//using AsyncPlate.Core.DTOs.Inventory;
//using AsyncPlate.Core.DTOs.Recipe;
using AsyncPlate.Domain.Entities;
using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Application.Services.Implementation
{
    public class CategoryService: ICategoryService
    {

        private readonly ILogger<RecipeService> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<AddCategoryRequestDTO> _validator1;
        private readonly IEmailService _emailService;
        private readonly IMediaService _mediaService;
        public CategoryService(ILogger<RecipeService> logger, IMapper mapper, IUnitOfWork unitOfWork, IValidator<AddCategoryRequestDTO> validator1, 
            IMediaService mediaService, IEmailService emailService)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _validator1 = validator1;
            _mediaService = mediaService;
            _emailService = emailService;
        }

        public async Task<CategoryResponseDTO> AddCategoryAsync(AddCategoryRequestDTO categoryRequestDTO)
        {
            //validate dto 
            //mapping
            //add to db
            //save and mapping then return 

            var validationResult = await _validator1.ValidateAsync(categoryRequestDTO);

            if (!validationResult.IsValid)
            {
                var errorsDictionary = validationResult.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray()
                    );

                throw new Exceptions.ValidationException(errorsDictionary);
            }

            var category = _mapper.Map<Category>(categoryRequestDTO);
            category.Name = category.Name.ToLower();

            var exists = await _unitOfWork.categories.AnyCategoryAsync(categoryRequestDTO.Name.ToLower());

            if (exists)
            {
                _logger.LogWarning("Attempt to create a category that already exists: {Name}", categoryRequestDTO.Name);
                throw new Exceptions.BadRequestException("Category already exists.");
            }
            if (categoryRequestDTO.OfferId != null)
            {
                var offerExists = await _unitOfWork.offers.GetByIdAsync(categoryRequestDTO.OfferId);
                if (offerExists == null)
                {
                    _logger.LogWarning("Attempt to create a category with a non-existent offer ID: {OfferId}", categoryRequestDTO.OfferId);
                    throw new Exceptions.BadRequestException("Offer with the provided ID does not exist.");
                }
            }

            //image upload 
            var imageUrl = await _mediaService.UploadImageAsync(categoryRequestDTO.ImageUrl, "categories");

            await _unitOfWork.categories.AddAsync(category);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Category created successfully: {Name}", category.Name);
            //RELOAD THE RELATED RELATIONSHIP : 
            //the mapper will not be able to map the supplier from the supplier_id in the dto so we 
            //need to tell him to get the supplier from the db first => lazy/explicit/eager loading

            category = await _unitOfWork.categories.GetCategoryWithRelatedDataAsync(category.Id);
            var responseDTO = _mapper.Map<CategoryResponseDTO>(category);
            return responseDTO;
        }

        public async Task<CategoryResponseDTO> GetCategoryByIdAsync(string categoryId)
        {
            var category = await _unitOfWork.categories.GetCategoryWithRelatedDataAsync(categoryId);
            if (category == null)
            {
                _logger.LogWarning("Category not found with ID: {CategoryId}", categoryId);
                throw new Exceptions.NotFoundException("Category not found.");
            }
            var responseDTO = _mapper.Map<CategoryResponseDTO>(category);
            return responseDTO;
        }
    }
}
