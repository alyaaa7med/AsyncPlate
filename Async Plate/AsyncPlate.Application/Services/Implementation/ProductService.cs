using AsyncPlate.Application.Common.DTOs;
using AsyncPlate.Application.Common.Extenstions;
using AsyncPlate.Application.Constants;
using AsyncPlate.Application.DTOs.Authentication;
using AsyncPlate.Application.DTOs.Menu;
using AsyncPlate.Application.DTOs.Product;
using AsyncPlate.Application.DTOs.Recipe;
using AsyncPlate.Application.Interfaces;
using AsyncPlate.Application.Interfaces.Services;
using AsyncPlate.Application.Services.Interfaces;
using AsyncPlate.Domain.Entities;
using AutoMapper;
using FluentValidation;
using Hangfire.Dashboard;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Application.Services.Implementation
{
    public class ProductService : IProductService
    {

        private readonly ILogger<ProductService> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<AddProductRequestDTO> _validator1;
        private readonly IEmailService _emailService;
        private readonly IRealtimeService _realtimeService;
        public ProductService(ILogger<ProductService> logger, IMapper mapper, IUnitOfWork unitOfWork,
            IRealtimeService realtimeService,IValidator<AddProductRequestDTO> validator1, IEmailService emailService)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _validator1 = validator1;
            _emailService = emailService;
            _realtimeService = realtimeService;
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

            _unitOfWork.products.Add(product);
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

        public async Task MakeProductUnAvailableAsync(string productId)
        {
            var product = await _unitOfWork.products.GetByIdAsync(productId);

            if (product == null)
            {
                _logger.LogWarning("Product with id {ProductId} not found", productId);
                throw new Exceptions.NotFoundException("Product not found");
            }

            product.IsAvailable = false;


            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("Availability for product {ProductId} changed to {Availability}", productId, product.IsAvailable);

            //immediatly update the menuitem ..
            var updatedMenuItem = new MenuRealtimeUpdateDTO
            {
                MenuItemId = product.Id,
                IsAvailable = false,
                HasOffer = false,
                FinalPrice = product.BasePrice,
                IsDeleted = false
            };

            await _realtimeService.SendToGroupAsync("Customers", RealtimeEvents.MenuUpdated, updatedMenuItem);
        }


        public async Task DeleteProductAsync(string productId)
        {
            var product = await _unitOfWork.products.GetByIdAsync(productId);

            if (product == null)
            {
                _logger.LogWarning("Product with id {ProductId} not found", productId);
                throw new Exceptions.NotFoundException("Product not found");
            }

            _unitOfWork.products.Delete(product);

            await _unitOfWork.SaveChangesAsync();


            //immediatly update the menu 
            var updatedMenuItem = new MenuRealtimeUpdateDTO
            {
                MenuItemId = product.Id,
                IsAvailable = false,
                HasOffer = false,
                FinalPrice = product.BasePrice,
                IsDeleted = true
            };

            await _realtimeService.SendToGroupAsync("Customers", RealtimeEvents.MenuUpdated, updatedMenuItem);

            _logger.LogInformation("Product with id {ProductId} deleted successfully", productId);
        }

        public async Task<PagedResult<ProductResponseDTO>> GetAllProductsAsync(int pageNumber, int pageSize)
        {
            var productsquery = _unitOfWork.products.GetAllWithCatgeorySummary();

            var pagedResult = await productsquery.ToPagedResultAsync(pageNumber, pageSize);

            var responseDTOs = _mapper.Map<IEnumerable<ProductResponseDTO>>(pagedResult.Items);
            _logger.LogInformation("Retrieved {Count} inventories (Page {PageNumber} of {TotalPages})", responseDTOs.Count(), pageNumber, pagedResult.TotalPages);

            return new PagedResult<ProductResponseDTO>
            {
                Items = responseDTOs,
                TotalCount = pagedResult.TotalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = pagedResult.TotalPages
            };


        }

        public async Task<IEnumerable<ProductResponseDTO>> GetProductsByCategoryAsync(string categoryId)
        {
            var category = await _unitOfWork.categories.GetByIdAsync(categoryId);

            if (category == null)
            {
                _logger.LogWarning("Category with id {CategoryId} not found", categoryId);
                throw new Exceptions.NotFoundException("Category not found");
            }

            var products = await _unitOfWork.products.GetProductsByCategoryIdAsync(categoryId);

            _logger.LogInformation("Retrieved  products for category {CategoryId}", categoryId);

            return _mapper.Map<IEnumerable<ProductResponseDTO>>(products);
        }

        public async Task<IEnumerable<ProductResponseDTO>> GetAvailableProductsAsync()
        {
            var products = await _unitOfWork.products.GetAvailableProductsAsync();

            _logger.LogInformation("Retrieved  available products");
            return _mapper.Map<IEnumerable<ProductResponseDTO>>(products);
        }

        public async Task<IEnumerable<ProductResponseDTO>> GetUnAvailableProductsAsync()
        {
            var products = await _unitOfWork.products.GetUnavailableProductsAsync();

            _logger.LogInformation("Retrieved  unavailable products");
            return _mapper.Map<IEnumerable<ProductResponseDTO>>(products);
        }

        public async Task<IEnumerable<ProductResponseDTO>> GetProductsByPriceRangeAsync(decimal minPrice, decimal maxPrice)
        {
            if (minPrice < 0)
            {
                throw new Exceptions.BadRequestException("Minimum price cannot be negative.");
            }
            if (maxPrice < minPrice)
            {
                throw new Exceptions.BadRequestException("Maximum price must be greater than or equal to the minimum price.");
            }

            var products = await _unitOfWork.products.GetProductsByPriceRangeAsync(minPrice, maxPrice);

            _logger.LogInformation("Retrieved products with price between {MinPrice} and {MaxPrice}", minPrice, maxPrice);
            return _mapper.Map<IEnumerable<ProductResponseDTO>>(products);
        }


        public async Task<IEnumerable<ProductResponseDTO>> GetBestSellerProductsAsync()
        {
            var products = await _unitOfWork.products.GetTopSellingProductsAsync();

            _logger.LogInformation("Retrieved  top selling products");
            return _mapper.Map<IEnumerable<ProductResponseDTO>>(products);
        }

    }
}
