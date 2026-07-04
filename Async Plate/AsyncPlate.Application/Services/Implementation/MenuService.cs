using AsyncPlate.Application.Common.DTOs;
using AsyncPlate.Application.Common.Extenstions;
using AsyncPlate.Application.DTOs.Menu;
using AsyncPlate.Application.Exceptions;
using AsyncPlate.Application.Interfaces;
using AsyncPlate.Application.Interfaces.Services;
using AsyncPlate.Application.Services.Interfaces;
using AsyncPlate.Domain.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Type = AsyncPlate.Domain.Entities.Type;

namespace AsyncPlate.Application.Services.Implementation
{
    public class MenuService : IMenuService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger<MenuService> _logger;
        private readonly IInventoryService _inventoryService;


        public MenuService(IUnitOfWork unitOfWork,IMapper mapper, ILogger<MenuService> logger, IInventoryService inventoryService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _inventoryService = inventoryService;
        }

        public async Task<PagedResult<MenuItemResponseDTO>> GetMenuAsync(MenuFilterDTO filterDTO)
        {
            _logger.LogInformation(
                "Retrieving menu. Category={CategoryId}, Type={Type}, Page={PageNumber}",
                filterDTO.CategoryId,
                filterDTO.Type,
                filterDTO.PageNumber);

            var query = _unitOfWork.products.GetMenuProducts();

            if (!string.IsNullOrWhiteSpace(filterDTO.CategoryId))
            {
                query = query.Where(p => p.CategoryId == filterDTO.CategoryId);
            }

            if (!string.IsNullOrWhiteSpace(filterDTO.Type))
            {
                if (!Enum.TryParse<Type>(filterDTO.Type, true, out var productType))
                {
                    throw new Exceptions.BadRequestException("Invalid product type.");
                }
                query = query.Where(p => p.Type == productType);
            }

            if (filterDTO.AvailableOnly == true)
            {
                query = query.Where(p => p.IsAvailable);
            }

            if (filterDTO.MinPrice.HasValue)
            {
                query = query.Where(p => p.BasePrice >= filterDTO.MinPrice.Value);
            }

            if (filterDTO.MaxPrice.HasValue)
            {
                query = query.Where(p => p.BasePrice <= filterDTO.MaxPrice.Value);
            }

            var pagedResult = await query.ToPagedResultAsync(filterDTO.PageNumber,filterDTO.PageSize);

            var menuItems = pagedResult.Items.Select(product =>
            {
                var dto = _mapper.Map<MenuItemResponseDTO>(product);

                dto.HasOffer = _unitOfWork.products.HasActiveOffer(product);
                dto.FinalPrice = _unitOfWork.products.GetFinalPrice(product);
                dto.IsOutOfStock = product.IsAvailable;

                return dto;
            }).ToList();

            return new PagedResult<MenuItemResponseDTO>
            {
                Items = menuItems,
                TotalCount = pagedResult.TotalCount,
                PageNumber = pagedResult.PageNumber,
                PageSize = pagedResult.PageSize
            };
        }

        public async Task<MenuDetailsResponseDTO> GetProductDetailsAsync(string productId)
        {
            _logger.LogInformation("Retrieving menu product details. ProductId: {ProductId}",productId);

            var product = await _unitOfWork.products.GetMenuProductByIdAsync(productId);

            if (product == null)
            {
                _logger.LogWarning( "Product not found. ProductId: {ProductId}",productId);

                throw new Exceptions.NotFoundException($"Product with id '{productId}' was not found.");
            }

            var response = _mapper.Map<MenuDetailsResponseDTO>(product);


            response.HasOffer = _unitOfWork.products.HasActiveOffer(product);
            response.FinalPrice = _unitOfWork.products.GetFinalPrice(product);
            response.IsOutOfStock = product.IsAvailable;

            return response;
        }
    }
}
