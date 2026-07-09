using AsyncPlate.Application.Constants;
using AsyncPlate.Application.DTOs.Notification;
using AsyncPlate.Application.Interfaces;
using AsyncPlate.Application.Interfaces.Jobs;
using AsyncPlate.Application.Interfaces.Services;
using AsyncPlate.Domain.Entities;
using AsyncPlate.Infrastructure.Data;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Infrastructure.Services.Jobs
{
    public class OfferJob : IOfferJob
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IRealtimeService _realtimeService;
        private readonly IMapper _mapper;
        public OfferJob(IUnitOfWork unitOfWork, IMapper mapper, IRealtimeService realtimeService)
        {
            _unitOfWork = unitOfWork;
            _realtimeService = realtimeService  ;
            _mapper = mapper;
        }

        public async Task SendNewOfferNotificationsAsync(string offerId)
        {
            var offer = await _unitOfWork.offers.GetByIdAsync(offerId);
            if (offer == null)
            {
                throw new Application.Exceptions.NotFoundException($"Offer with id = {offerId} not found");
            }   

            var vipUsers = await _unitOfWork.customers.GetVipCustomerUserIdsAsync();

            var notifications = vipUsers.Select(userId => new Notification
            {
                userId = userId,
                Title = "New Offer",
                Message = $"New offer available: {offer.Title} with {offer.DiscountPercentage}% discount!",
                Url = $"Offers/{offer.Id}"
            
            }).ToList();

            await _unitOfWork.notifications.AddRangeAsync(notifications);//batch insert for optimization
            await _unitOfWork.SaveChangesAsync();

            foreach (var notification in notifications)
            {
                var notificationDTO = _mapper.Map<NotificationResponseDTO>(notification);//no need to reload for the nav prop userid as
                                                                                         //the object i created has the userId 
                await _realtimeService.SendToUserAsync(notificationDTO.UserId, RealtimeEvents.NotificationReceived, notificationDTO);

            }
            //foreach (var userId in vipUsers)
            //{
            //    var notificationDTO = _mapper.Map < Notification > 
            //    await _notificationSender.SendToUserAsync(
            //        userId, "notfication for new offer on {offer.title}")
                    
            //    );
            //}

        }
    }

}
