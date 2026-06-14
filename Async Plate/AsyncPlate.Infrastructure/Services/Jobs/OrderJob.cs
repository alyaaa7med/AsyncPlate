using AsyncPlate.Application.DTOs.Notification;
using AsyncPlate.Application.Exceptions;
using AsyncPlate.Application.Interfaces;
using AsyncPlate.Application.Interfaces.Services;
using AsyncPlate.Application.Jobs;
using AsyncPlate.Domain.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Infrastructure.Services.Jobs
{
    public class OrderJob : IOrderJob
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotificationSender _notificationSender;
        private readonly IMapper _mapper;
        public OrderJob(IUnitOfWork unitOfWork, IMapper mapper, INotificationSender notificationSender)
        {
            _unitOfWork = unitOfWork;
            _notificationSender = notificationSender;
            _mapper = mapper;
        }

        public async Task SendNewOrderNotificationAsync(string orderId)
        {
            var order = await _unitOfWork.orders.GetByIdAsync(orderId);
            if (order == null)
            {
                throw new Application.Exceptions.NotFoundException($"Order with id = {orderId} not found");
            }


            var chefUserIds = await _unitOfWork.kitchenChefs.GetChefUserIdsAsync();

            var notifications = chefUserIds.Select(userId => new Notification
            {
                userId = userId,
                Title = "New Order To Cook",
                Message = $"Order #{order.Id} is confirmed and ready for cooking.",
                Url = $"Orders/{order.Id}"
            }).ToList();

            await _unitOfWork.notifications.AddRangeAsync(notifications);
            await _unitOfWork.SaveChangesAsync();

            //i just need to map only one notitfication as the userId not needed here 
            //i send per group not per user 

            var notificationDto = new NotificationResponseDTO
            {
                UserId = orderId,//it does not mean any thing other than unifing the response 
                Title = "New Order To Cook",
                Message = $"Order #{order.Id} is confirmed and ready for cooking.",
                Url = $"Orders/{order.Id}"
            };

            await _notificationSender.SendToGroupAsync("Chefs", notificationDto);
        }

        public async Task SendCookingOrderNotificationAsync(string orderId)
        {

            var order = await _unitOfWork.orders.GetByIdAsync(orderId);

            if (order == null)
            {
                throw new Application.Exceptions.NotFoundException($"Order with id = {orderId} not found");
            }
            if (order.CustomerId == null)
            {
                throw new Application.Exceptions.NotFoundException($"No customer assigned to this order...");
            }

            var notification = new Notification
            {
                userId = order.CustomerId,
                Title = "Cooking Order",
                Message = $"Order #{order.Id} is being prepared.",
                Url = $"Orders/{order.Id}"
            };

            await _unitOfWork.notifications.AddAsync(notification);
            await _unitOfWork.SaveChangesAsync();

            var notificationDTO = _mapper.Map<NotificationResponseDTO>(notification);

            await _notificationSender.SendToUserAsync(notification.userId, notificationDTO);

        }

        public async Task SendCompleteOrderNotificationAsync(string orderId)
        {

            var order = await _unitOfWork.orders.GetByIdAsync(orderId);

            if (order == null)
            {
                throw new Application.Exceptions.NotFoundException($"Order with id = {orderId} not found");
            }
            if (order.CustomerId == null)
            {
                throw new Application.Exceptions.NotFoundException($"No customer assigned to this order...");
            }

            var notification = new Notification
            {
                userId = order.CustomerId,
                Title = "Complete Order",
                Message = $"Order #{order.Id} is Ready to be delivered.",
                Url = $"Orders/{order.Id}"
            };

            await _unitOfWork.notifications.AddAsync(notification);
            await _unitOfWork.SaveChangesAsync();

            var notificationDTO = _mapper.Map<NotificationResponseDTO>(notification);

            await _notificationSender.SendToUserAsync(notification.userId, notificationDTO);
        }
    }
}
