using AsyncPlate.Application.Interfaces;
using AsyncPlate.Application.Interfaces.Services;
using AsyncPlate.Domain.Entities;
using AsyncPlate.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Infrastructure.Services
{
    public class OfferJob : IOfferJob
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotificationSender _notificationSender;

        public OfferJob(IUnitOfWork unitOfWork, INotificationSender notificationSender)
        {
            _unitOfWork = unitOfWork;
            _notificationSender = notificationSender;
        }

        public async Task SendOfferNotifications(string offerId)
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
                Message = $"New offer available: {offer.Title} with {offer.DiscountPercentage}% discount!"
            }).ToList();

            await _unitOfWork.notifications.AddRangeAsync(notifications);//batch insert for optimization
            await _unitOfWork.SaveChangesAsync();


            foreach (var userId in vipUsers)
            {
                await _notificationSender.SendToUserAsync(
                    userId,
                    $"New offer available: {offer.Title} with {offer.DiscountPercentage}% discount!"
                );
            }

        }
    }

}
