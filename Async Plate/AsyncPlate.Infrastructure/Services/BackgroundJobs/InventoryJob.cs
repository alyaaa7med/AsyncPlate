using AsyncPlate.Application.Constants;
using AsyncPlate.Application.DTOs.Notification;
using AsyncPlate.Application.Interfaces;
using AsyncPlate.Application.Interfaces.Jobs;
using AsyncPlate.Application.Interfaces.Services;
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
        private readonly IRealtimeService _realtimeService;
        private readonly IEmailService _emailService;
        public InventoryJob(IUnitOfWork unitOfWork, IRealtimeService realtimeService , IEmailService emailService)
        {
            _unitOfWork = unitOfWork;
            _realtimeService = realtimeService;
            _emailService = emailService;
        }

        public async Task SendLowStockInventoryNotificationAsync(string inventoryId)
        {
            var inventory = await _unitOfWork.inventories.GetByIdAsync(inventoryId);
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

            await _realtimeService.SendToGroupAsync("Chefs", RealtimeEvents.NotificationReceived, notificationDto);
            await _realtimeService.SendToGroupAsync("Admins", RealtimeEvents.NotificationReceived, notificationDto);

        }


        public async Task SendLowStockSuppliersEmailAsync()
        {
            var lowStockSuppliers = (await _unitOfWork.inventories.GetLowStockWithSuppliersAsync())
                .GroupBy(i => i.Supplier.ContactEmail)
                .ToDictionary(g => g.Key, g => g.ToList()); //{supplieremail, [list of his related inventory]}

            foreach (var dictionary in lowStockSuppliers)
            {
                var supplierEmail = dictionary.Key;
                string body = CreateBody(dictionary.Value);

                await _emailService.SendEmailAsync(
                    supplierEmail,
                    "Low Stock Items Email",
                    body
                );
            }


        }

        private string CreateBody(List<Inventory> inventories)
        {
            var builder = new StringBuilder();

            builder.AppendLine("<h2>Daily Inventory Replenishment Request</h2>");

            builder.AppendLine("<p>Please provide the following quantities:</p>");

            builder.AppendLine(@"
                 <table border='1' cellpadding='5'>
                 <tr>
                <th>Item</th>
                <th>Least Needed Quantity</th>
            </tr>");

            foreach (var inventory in inventories)
            {
                var leatneededQuantity =
                Math.Max(0, inventory.MinStockLevel - inventory.CurrentStock);

                builder.AppendLine($@"
            <tr>
                <td>{inventory.Name}</td>
                <td>{leatneededQuantity}</td>
            </tr>");
            }

            builder.AppendLine("</table>");

            builder.AppendLine("<p>Thank you.</p>");
            builder.AppendLine("<p>AsyncPlate Team</p>");

            return builder.ToString();
        }

    }
}
