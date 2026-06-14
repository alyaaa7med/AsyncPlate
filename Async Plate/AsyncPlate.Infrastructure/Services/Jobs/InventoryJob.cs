using AsyncPlate.Application.DTOs.Notification;
using AsyncPlate.Application.Interfaces;
using AsyncPlate.Application.Interfaces.Services;
using AsyncPlate.Application.Jobs;
using AsyncPlate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Infrastructure.Services.Jobs
{
    public class InventoryJob : IInventoryJob
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotificationSender _notificationSender;

        public InventoryJob(IUnitOfWork unitOfWork, INotificationSender notificationSender)
        {
            _unitOfWork = unitOfWork;
            _notificationSender = notificationSender;
        }

        public async Task SendLowStockInventoryNotification(string inventoryId)
        {
            var inventory = await _unitOfWork.orders.GetByIdAsync(inventoryId);
            if (inventory == null)
            {
                throw new Application.Exceptions.NotFoundException($"Inventory with id = {inventoryId} not found");
            }


            var chefUserIds = await _unitOfWork.kitchenChefs.GetChefUserIdsAsync();
            var adminUserIds = await _unitOfWork.admins.GetAdminUserIdsAsync();

            var notifications = chefUserIds.Concat(adminUserIds)
                                           .Select(userId => new Notification
                                           {
                                               userId = userId,
                                               Title = "Low Stock Inventory",
                                               Message = $"Inventory #{inventory.Id} is below min stock.",
                                               Url = $"Inventories/{inventory.Id}"
                                           }).ToList();

            await _unitOfWork.notifications.AddRangeAsync(notifications);
            await _unitOfWork.SaveChangesAsync();

            var notificationDto = new NotificationResponseDTO
            {
                UserId = inventoryId,//it does not mean any thing other than unifing the response 
                Title = "Low Stock Inventory",
                Message = $"Inventory #{inventory.Id} is below stock.",
                Url = $"Inventories/{inventory.Id}"
            };

            await _notificationSender.SendToGroupAsync("Chefs", notificationDto);
            await _notificationSender.SendToGroupAsync("Admins", notificationDto);

        }

    }
}
