using AsyncPlate.Application.Common.DTOs;
using AsyncPlate.Application.DTOs.Notification;
using AsyncPlate.Domain.Entities;
using Hangfire.Dashboard;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Application.Services.Interfaces
{
    public interface INotificationService
    {
        Task<PagedResult<NotificationResponseDTO>> GetAllNotificationsAsync(string userId, int pageNumber, int pageSize);
        Task<NotificationResponseDTO> GetNotificationByIdAsync(string userId, string notificationId);
        Task<NotificationResponseDTO> MarkNotificationAsReadAsync(string userId, string notificationId);

    }
}
