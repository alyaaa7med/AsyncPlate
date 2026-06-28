using AsyncPlate.Application.DTOs.ProductExtra;
using AsyncPlate.Application.Exceptions;
using AsyncPlate.Application.Interfaces;
using AsyncPlate.Application.Services.Interfaces;
using AsyncPlate.Domain.Entities;
using AutoMapper;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AsyncPlate.Application.Services.Implementation
{
    public class ProductExtraService : IProductExtraService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<AddProductExtraDTO> _addValidator;
        private readonly IValidator<UpdateProductExtrasDTO> _updateValidator;
        private readonly ILogger<ProductExtraService> _logger;
        private readonly IMapper _mapper;

        public ProductExtraService(IUnitOfWork unitOfWork, ILogger<ProductExtraService> logger,
            IMapper mapper, IValidator<AddProductExtraDTO> addValidator, IValidator<UpdateProductExtrasDTO> updateValidator
            )
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _addValidator = addValidator;
            _updateValidator = updateValidator;
            _logger = logger;
        }

        public async Task<ProductWithExtrasDTO> AddExtrasAsync(string productId, AddProductExtraDTO dto)
        {
            var validationResult = await _addValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                var errorsDictionary = validationResult.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray());

                throw new Exceptions.ValidationException(errorsDictionary);
            }

            var product = await _unitOfWork.products.GetByIdAsync(productId);

            if (product == null)
                throw new Exceptions.NotFoundException("Product not found.");

            var extraIds = dto.ExtraProductIds.Distinct().ToList();

            if (extraIds.Contains(productId))
                throw new Exceptions.BadRequestException("A product cannot be an extra for itself.");

           
            var extraProductsIds = await _unitOfWork.products.GetByIdsAsync(extraIds);

            if (extraProductsIds.Count < dto.ExtraProductIds.Count)
            {
                throw new Exceptions.NotFoundException($"some of the provided extra products is not found");
            }

            var invalidProducts = await _unitOfWork.products.GetInvalidExtraProductNamesAsync(extraIds);

            if (invalidProducts.Any())
            {
                throw new Exceptions.BadRequestException( $"The following products are not extra products: {string.Join(", ", invalidProducts)}");
            }

            var productExtras = new List<ProductExtra>();

            foreach (var extraProductId in extraProductsIds)
            {
                productExtras.Add(new ProductExtra {ProductId = productId,ExtraProductId = extraProductId });
            }

            _unitOfWork.ProductExtras.AddRange(productExtras);

            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("{Count} extra products added to product {ProductId}.",extraProductsIds.Count,productId);


            var productswithextras = await _unitOfWork.products.GetProductWithExtrasAsync(productId);

            return _mapper.Map<ProductWithExtrasDTO>(productswithextras);
        }

        public async Task<ProductWithExtrasDTO> UpdateExtrasAsync(string productId, UpdateProductExtrasDTO dto)
        {
            var validationResult = await _updateValidator.ValidateAsync(dto);

            if (!validationResult.IsValid)
            {
                var errorsDictionary = validationResult.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray());

                throw new Exceptions.ValidationException(errorsDictionary);
            }

            var product = await _unitOfWork.products.GetByIdAsync(productId);

            if (product == null)
                throw new Exceptions.NotFoundException("Product not found.");

            var extraIds = dto.ExtraProductIds.Distinct().ToList();

            if (extraIds.Contains(productId))
                throw new Exceptions.BadRequestException("A product cannot be an extra for itself.");

            var existingProducts = await _unitOfWork.products.GetByIdsAsync(extraIds);

            if (existingProducts.Count < extraIds.Count)
                throw new NotFoundException("Some of the provided extra products were not found.");

            var invalidProducts = await _unitOfWork.products.GetInvalidExtraProductNamesAsync(extraIds);

            if (invalidProducts.Any())
            {
                throw new Exceptions.BadRequestException($"The following products are not extra products: {string.Join(", ", invalidProducts)}");
            }

            //current relations in DB
            var currentRelations = await _unitOfWork.ProductExtras.GetByProductIdAsync(productId);//p pextra
           
            _unitOfWork.ProductExtras.DeleteRange(currentRelations);

            var productExtras = new List<ProductExtra>();

            foreach (var extraProductId in existingProducts)
            {
                productExtras.Add(new ProductExtra { ProductId = productId, ExtraProductId = extraProductId });
            }

            _unitOfWork.ProductExtras.AddRange(productExtras);

            await _unitOfWork.SaveChangesAsync();

            var productswithextras = await _unitOfWork.products.GetProductWithExtrasAsync(productId);

            return _mapper.Map<ProductWithExtrasDTO>(productswithextras);
        }

        public async Task<ProductWithExtrasDTO> DeleteExtraAsync(string productId, string extraProductId)
        {
            var product = await _unitOfWork.products.GetByIdAsync(productId);

            if (product == null)
                throw new Exceptions.NotFoundException("Product not found.");

            var relation = await _unitOfWork.ProductExtras.GetProductExtraAsync(productId, extraProductId);

            if (relation == null)
                throw new Exceptions.NotFoundException("The extra product is not related to this product.");

            _unitOfWork.ProductExtras.Delete(relation);

            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Extra product {ExtraProductId} removed from product {ProductId}.",extraProductId,productId);
            var productswithextras = await _unitOfWork.products.GetProductWithExtrasAsync(productId);

            return _mapper.Map<ProductWithExtrasDTO>(productswithextras);
        }


        public async Task DeleteAllExtrasAsync(string productId)
        {
            var product = await _unitOfWork.products.GetByIdAsync(productId);

            if (product == null)
                throw new Exceptions.NotFoundException("Product not found.");

            var relations = await _unitOfWork.ProductExtras.GetByProductIdAsync(productId);

            if (!relations.Any())
                throw new Exceptions.BadRequestException("This product has no extras.");

            _unitOfWork.ProductExtras.DeleteRange(relations);

            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("All extras removed from product {ProductId}.",productId);

        }


        public async Task<IEnumerable<ProductExtraDTO>> GetExtrasByProductIdAsync(string productId)
        {

            var product = await _unitOfWork.products.GetByIdAsync(productId);

            if (product == null)
                throw new Exceptions.NotFoundException("Product not found.");

            return await _unitOfWork.ProductExtras.GetExtrasByProductIdAsync(productId);

        }
    }
}