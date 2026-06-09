using AsyncPlate.Application.Common.DTOs;
using AsyncPlate.Application.DTOs.Offer;
using AsyncPlate.Application.Interfaces;
using AsyncPlate.Application.Interfaces.Services;
using AsyncPlate.Application.Services.Interfaces;
//using AsyncPlate.Core.DTOs.Inventory;
//using AsyncPlate.Core.Entities;
//using AsyncPlate.Core.Interfaces.Repositories;
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
    public class OfferService : IOfferService
    {
        private readonly ILogger<IOfferService> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<AddOfferRequestDTO> _validator1;
        private readonly IValidator<UpdateOfferRequestDTO> _validator2;
        private readonly INotificationSender _notificationSender;

        public OfferService(ILogger<IOfferService> logger, IMapper mapper, IUnitOfWork unitOfWork,
            IValidator<AddOfferRequestDTO> validator1, IValidator<UpdateOfferRequestDTO> validator2
            , INotificationSender notificationSender)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _validator1 = validator1;
            _validator2 = validator2;
            _notificationSender = notificationSender;
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
            await _unitOfWork.offers.AddAsync(offer);

            
            // get vip users
            var vipCustomerUserIds = await _unitOfWork.customers.GetVipCustomerUserIdsAsync();

            // create notifications
            var notifications = vipCustomerUserIds.Select(userId => new Notification
            {
                userId = userId,
                Message = $"New offer available: {offer.Title} with {offer.DiscountPercentage}% discount!"
            }).ToList();

            await _unitOfWork.notifications.AddRangeAsync(notifications);
            await _unitOfWork.SaveChangesAsync();

            // send realtime notifications
            foreach (var userId in vipCustomerUserIds)
            {
                _logger.LogInformation("sending to user with ID: {UserId}", userId);
                await _notificationSender.SendToUserAsync(
                    userId,
                    $"New offer available: {offer.Title} with {offer.DiscountPercentage}% discount!"
                );

            }
            return _mapper.Map<OfferResponseDTO>(offer);
        }

        public async Task<OfferResponseDTO> GetOfferByIdAsync(string id)
        {
            throw new NotImplementedException();
        }

        public async Task<OfferResponseDTO> DeleteOfferByIdAsync(string id)
        {
            throw new NotImplementedException();
        }
        public async Task<PagedResult<OfferResponseDTO>> GetAllOffersAsync(OfferFilterDTO offerFilter)
        {
            throw new NotImplementedException();
        }


        public async Task<OfferResponseDTO> UpdateOfferAsync(UpdateOfferRequestDTO offerRequestDTO)
        {
            throw new NotImplementedException();
        }


    }
}
