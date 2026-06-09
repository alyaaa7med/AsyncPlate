using AsyncPlate.Application.Common.DTOs;
using AsyncPlate.Application.DTOs.Offer;
using AsyncPlate.Application.Interfaces;
using AsyncPlate.Application.Interfaces.Services;
using AsyncPlate.Application.Services.Interfaces;
using AsyncPlate.Domain.Entities;
using AutoMapper;
using FluentValidation;
using Hangfire;
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
        private readonly IOfferJob _offerJob;

        public OfferService(ILogger<IOfferService> logger, IMapper mapper, IUnitOfWork unitOfWork,
            IValidator<AddOfferRequestDTO> validator1, IValidator<UpdateOfferRequestDTO> validator2
            , INotificationSender notificationSender, IOfferJob offerJob)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _validator1 = validator1;
            _validator2 = validator2;
            _notificationSender = notificationSender;
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

            await _unitOfWork.offers.AddAsync(offer);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Offer with ID {OfferId} created successfully.", offer.Id);

            //trigger the offer job to send notifications
            BackgroundJob.Enqueue<IOfferJob>(job => job.SendOfferNotifications(offer.Id));

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
