using AsyncPlate.Application.Common.DTOs;
using AsyncPlate.Application.Common.Extenstions;
using AsyncPlate.Application.DTOs.Recipe;
using AsyncPlate.Application.Interfaces;
using AsyncPlate.Application.Interfaces.Services;
using AsyncPlate.Application.Services.Interfaces;
using AsyncPlate.Domain.Entities;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Application.Services.Implementation
{
    public class RecipeService : IRecipeService
    {

        private readonly ILogger<RecipeService> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<AddRecipeRequestDTO> _validator1;
        private readonly IValidator<UpdateRecipeRequestDTO> _validator2;
        private readonly IEmailService _emailService;

        public RecipeService(ILogger<RecipeService> logger, IMapper mapper, IUnitOfWork unitOfWork, IValidator<AddRecipeRequestDTO> validator1, IValidator<UpdateRecipeRequestDTO> validator2   , IEmailService emailService)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _validator1 = validator1;
            _validator2 = validator2;
            _emailService = emailService;
        }
        public async Task<RecipeResponseDTO> AddRecipeAsync(AddRecipeRequestDTO addRecipeRequestDTO)
        {
            var validationResult = await _validator1.ValidateAsync(addRecipeRequestDTO);
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

            //check if product and inventory exist
            var product = await _unitOfWork.products.GetByIdAsync(addRecipeRequestDTO.ProductId);
            if (product == null)
            {
                _logger.LogWarning("Product with id {ProductId} not found", addRecipeRequestDTO.ProductId);
                throw new Exceptions.NotFoundException("Product not found");
            }
            var inventory = await _unitOfWork.inventories.GetByIdAsync(addRecipeRequestDTO.InventoryId);
            if (inventory == null)
            {
                _logger.LogWarning("Inventory with id {InventoryId} not found", addRecipeRequestDTO.InventoryId);
                throw new Exceptions.NotFoundException("Inventory not found");
            }

            var recipeExist = await _unitOfWork.recipes.GetRecipeWithInventoryAndProductAsync(addRecipeRequestDTO.InventoryId, addRecipeRequestDTO.ProductId);
            if (recipeExist != null)
            {
                _logger.LogWarning("Recipe with inventory id {InventoryId} and product id {ProductId} already exists", addRecipeRequestDTO.InventoryId, addRecipeRequestDTO.ProductId);
                throw new Exceptions.ValidationException(new Dictionary<string, string[]>
                {
                    { "Recipe", new[] { "Recipe already exists" }}
                });
            }

            var recipe = _mapper.Map<Recipe>(addRecipeRequestDTO);
        

             _unitOfWork.recipes.Add(recipe);
            await _unitOfWork.SaveChangesAsync();

            //reload recipe with inventory and product
            recipe = await _unitOfWork.recipes.GetRecipeWithInventoryAndProductAsync(addRecipeRequestDTO.InventoryId, addRecipeRequestDTO.ProductId);
            var responseDTO = _mapper.Map<RecipeResponseDTO>(recipe);
            return responseDTO;
        }


       
        public async Task<RecipeResponseDTO> UpdateRecipeAsync(string inventoryId,string productId, UpdateRecipeRequestDTO updateRecipeRequestDTO)
        {
            //validate dto
            //check if recipe exist(inventory and product)
            //mapping 
            //update
            //return respnse
            var validationResult = await _validator2.ValidateAsync(updateRecipeRequestDTO);
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

            var recipe = await _unitOfWork.recipes.GetRecipeWithInventoryAndProductAsync(inventoryId, productId);
            if (recipe == null)
            {
                _logger.LogWarning("Recipe with inventory id {InventoryId} and product id {ProductId} not found", inventoryId, productId);
                throw new Exceptions.NotFoundException("Recipe not found");
            }

            _mapper.Map(updateRecipeRequestDTO, recipe);
            if (recipe.Quantity > recipe.Inventory.CurrentStock)
            {
                _logger.LogWarning("Not enough stock in inventory {InventoryId} for product {ProductId}", inventoryId, productId);
                throw new Exceptions.ValidationException(new Dictionary<string, string[]>
                {
                    { "Quantity", new[] { "Not enough stock in inventory" }}
                });
            }

            _unitOfWork.recipes.Update(recipe);
            await _unitOfWork.SaveChangesAsync();

            //reload recipe with inventory and product
            recipe = await _unitOfWork.recipes.GetRecipeWithInventoryAndProductAsync(inventoryId, productId);
            var responseDTO = _mapper.Map<RecipeResponseDTO>(recipe);
            return responseDTO;

        }

        public async Task<RecipeResponseDTO> DeleteRecipeAsync(string inventoryId, string productId)
        {
            var recipe = await _unitOfWork.recipes.GetRecipeWithInventoryAndProductAsync(inventoryId, productId);
            if (recipe == null)
            {
                _logger.LogWarning("Recipe with inventory id {InventoryId} and product id {ProductId} not found", inventoryId, productId);
                throw new Exceptions.NotFoundException("Recipe not found");
            }
           
            _unitOfWork.recipes.Delete(recipe);
            await _unitOfWork.SaveChangesAsync();
            var responseDTO = _mapper.Map<RecipeResponseDTO>(recipe);
            return responseDTO;
        }
        public async Task<PagedResult<RecipeResponseDTO>> GetAllRecipesAsync(RecipeFilterDTO filterDto)
        {
            var recipesQuery = _unitOfWork.recipes.GetAllRecipes();

            if(!string.IsNullOrEmpty(filterDto.ProductName))
                recipesQuery = recipesQuery.Where(r => r.Product.Name.Contains(filterDto.ProductName));

            if(!string.IsNullOrEmpty(filterDto.InventoryName))
                recipesQuery = recipesQuery.Where(r => r.Inventory.Name.Contains(filterDto.InventoryName));

            var pagedResult = await recipesQuery.ToPagedResultAsync(filterDto.PageNumber, filterDto.PageSize);

            var responseDTOs = _mapper.Map<IEnumerable<RecipeResponseDTO>>(pagedResult.Items);


            _logger.LogInformation("Retrieved {Count} recipes", responseDTOs.Count());
            return new PagedResult<RecipeResponseDTO>
            {
                Items = responseDTOs,
                TotalCount = pagedResult.TotalCount,
                PageNumber = filterDto.PageNumber,
                PageSize = filterDto.PageSize,
                TotalPages = pagedResult.TotalPages
            };
        }
       
     
    }

}
