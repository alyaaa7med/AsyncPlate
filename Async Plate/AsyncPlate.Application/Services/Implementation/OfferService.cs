using AsyncPlate.Application.Common.DTOs;
using AsyncPlate.Application.Common.Extenstions;
using AsyncPlate.Application.Constants;
using AsyncPlate.Application.DTOs.Menu;
using AsyncPlate.Application.DTOs.Offer;
using AsyncPlate.Application.Exceptions;
using AsyncPlate.Application.Interfaces;
using AsyncPlate.Application.Interfaces.Jobs;
using AsyncPlate.Application.Interfaces.Services;
using AsyncPlate.Application.Services.Interfaces;
using AsyncPlate.Domain.Entities;
using AutoMapper;
using FluentValidation;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Application.Services.Implementation
{
    public class OfferService : IOfferService
    {
        private readonly ILogger<OfferService> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<AddOfferRequestDTO> _validator1;
        private readonly IRealtimeService _realtimeService;
        private readonly IOfferJob _offerJob;

        public OfferService(ILogger<OfferService> logger, IMapper mapper, IUnitOfWork unitOfWork,
            IValidator<AddOfferRequestDTO> validator1
            , IRealtimeService realtimeService, IOfferJob offerJob)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _validator1 = validator1;
            _realtimeService = realtimeService;
            _offerJob = offerJob;
        }

        public async Task<OfferResponseDTO> AddOfferAsync(AddOfferRequestDTO offerRequestDTO)
        {

            var validationResult = await _validator1.ValidateAsync(offerRequestDTO);
            if (!validationResult.IsValid)
            {
                var errorsDictionary = validationResult.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray()
                    );
                _logger.LogWarning("Validation failed for AddOfferRequestDTO: {Errors}", errorsDictionary);
                throw new Exceptions.ValidationException(errorsDictionary);
            }


            var offer = _mapper.Map<Offer>(offerRequestDTO);

            var categories = await _unitOfWork.categories.GetCategoriesByIdsAsync(offerRequestDTO.CategoryIds);
            /*
            //chef if categories count is less than categoryIds count then some categories are not found
            if (categories.Count() < offerRequestDTO.CategoryIds.Count)
            {
                var foundCategoryIds = categories.Select(c => c.Id).ToHashSet();
                var notFoundCategoryIds = offerRequestDTO.CategoryIds.Where(id => !foundCategoryIds.Contains(id));
                throw new Exceptions.NotFoundException($"Categories with IDs {string.Join(", ", notFoundCategoryIds)} not found.");
            }
            */

            offer.Categories = categories;

            _unitOfWork.offers.Add(offer);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Offer with ID {OfferId} created successfully.", offer.Id);

            //trigger the offer job to send notifications
            BackgroundJob.Enqueue<IOfferJob>(job => job.SendNewOfferNotificationsAsync(offer.Id));

            return _mapper.Map<OfferResponseDTO>(offer);
        }

        public async Task<OfferResponseDTO> GetOfferByIdAsync(string offerId)
        {
            _logger.LogInformation("Retrieving offer {OfferId}.", offerId);

            var offer = await _unitOfWork.offers.GetOfferWithCategoryAsync(offerId);

            if (offer == null)
            {
                _logger.LogWarning("Offer {OfferId} not found.", offerId);

                throw new NotFoundException($"Offer with id '{offerId}' was not found.");
            }

            return _mapper.Map<OfferResponseDTO>(offer);
        }

        public async Task<OfferResponseDTO> InactivateOfferAsync(string offerId)
        {
            _logger.LogInformation("Inactivating offer {OfferId}.", offerId);

            var offer = await _unitOfWork.offers.GetOfferWithCategoryAsync(offerId);

            if (offer == null)
            {
                _logger.LogWarning("Offer {OfferId} not found.", offerId);

                throw new NotFoundException($"Offer with id '{offerId}' was not found.");
            }

            offer.IsActive = false;

            await _unitOfWork.SaveChangesAsync();

            var menuItems = await _unitOfWork.products.GetMenuProducts().Select(m => m.Id).ToListAsync();

            //immediatly update the menu 
            var dto = new MenuBulkRealtimeUpdateDTO
            {
                MenuItems = _unitOfWork.products.GetMenuProducts().Select(p => new MenuRealtimeUpdateDTO
                {
                    MenuItemId = p.Id,
                    IsAvailable = p.IsAvailable,
                    HasOffer = false,
                    FinalPrice = p.BasePrice,
                    IsDeleted = false
                }).ToList()
            };

            //immediatly update the menu 
            await _realtimeService.SendToGroupAsync("Customers", RealtimeEvents.MenuBulkUpdated, dto);
                 
          
            return _mapper.Map<OfferResponseDTO>(offer);
        }

        public async Task<PagedResult<OfferResponseDTO>> GetAllOffersAsync(OfferFilterDTO offerFilterDTO)
        {
            var query = _unitOfWork.offers.GetAllOffers();

            if (!string.IsNullOrWhiteSpace(offerFilterDTO.CategoryName))
                query = _unitOfWork.offers.FilterByCategory(query, offerFilterDTO.CategoryName);

            if (offerFilterDTO.Percentage.HasValue)
                query = _unitOfWork.offers.FilterByPercentage(query, offerFilterDTO.Percentage.Value);

            if (offerFilterDTO.IsActive == true)
                query = _unitOfWork.offers.FilterActive(query);

            var pagedResult = await query.ToPagedResultAsync(
                offerFilterDTO.PageNumber,
                offerFilterDTO.PageSize);

            return new PagedResult<OfferResponseDTO>
            {
                Items = _mapper.Map<List<OfferResponseDTO>>(pagedResult.Items),
                TotalCount = pagedResult.TotalCount,
                PageNumber = pagedResult.PageNumber,
                PageSize = pagedResult.PageSize,
                TotalPages = pagedResult.TotalPages
            };
        }
    }
}


