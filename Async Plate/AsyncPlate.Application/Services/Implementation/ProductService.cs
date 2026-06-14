using AsyncPlate.Application.DTOs.Product;
using AsyncPlate.Application.DTOs.Recipe;
using AsyncPlate.Application.Interfaces;
using AsyncPlate.Application.Interfaces.Services;
using AsyncPlate.Application.Services.Interfaces;
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
    public class ProductService : IProductService
    {

        private readonly ILogger<RecipeService> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<AddProductRequestDTO> _validator1;
        private readonly IEmailService _emailService;
        public ProductService(ILogger<RecipeService> logger, IMapper mapper, IUnitOfWork unitOfWork, IValidator<AddProductRequestDTO> validator1, IEmailService emailService)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _validator1 = validator1;
            _emailService = emailService;
        }
        public async Task<ProductResponseDTO> AddProductAsync(AddProductRequestDTO productRequestDTO)
        {
            //validate dto 
            //mapping
            //save
            //return response dto
            var validationResult = await _validator1.ValidateAsync(productRequestDTO);
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

            //check if category 
            var category = await _unitOfWork.categories.GetByIdAsync(productRequestDTO.CategoryId);
            if (category == null)
            {
                _logger.LogWarning("Category with id {CategoryId} not found", productRequestDTO.CategoryId);
                throw new Exceptions.NotFoundException("Category not found");
            }

            var product = _mapper.Map<Product>(productRequestDTO);
            await _unitOfWork.products.AddAsync(product);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("Product with id {ProductId} created successfully", product.Id);
            //reolad the product with category details
            product = await _unitOfWork.products.GetProductWithCategoryAsync(product.Id);
            var responseDTO = _mapper.Map<ProductResponseDTO>(product);
            return responseDTO;

        }

        public async Task<ProductResponseDTO> GetProductByIdAsync(string productId)
        {
            var product = await _unitOfWork.products.GetProductWithCategoryAsync(productId);
            if (product == null)
            {
                _logger.LogWarning("Product with id {ProductId} not found", productId);
                throw new Exceptions.NotFoundException("Product not found");
            }
            //reload the product with category details

            product = await _unitOfWork.products.GetProductWithCategoryAsync(product.Id);

            var responseDTO = _mapper.Map<ProductResponseDTO>(product);
            return responseDTO;
        }

        public async Task<IEnumerable<RecipeListDTO>> GetRecipeByProductIdAsync(string productId)
        {
            //validate id 
            //get recipes of product
            //mapping 
            var product = await _unitOfWork.products.GetByIdAsync(productId);
            if (product == null)
            {
                _logger.LogWarning("Product with id {ProductId} not found", productId);
                throw new Exceptions.NotFoundException("Product not found");
            }
            var recipeList = await _unitOfWork.recipes.GetRecipeByProductIdAsync(productId);
            var responseDTOs = _mapper.Map<IEnumerable<RecipeListDTO>>(recipeList);
            _logger.LogInformation("Retrieved {Count} recipes for product id {ProductId}", responseDTOs.Count(), productId);
            return responseDTOs;
        }
    }
}
