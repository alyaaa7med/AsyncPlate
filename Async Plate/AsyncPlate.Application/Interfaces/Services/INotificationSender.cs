using AsyncPlate.Application.DTOs.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Application.Interfaces.Services
{
    public interface INotificationSender
    {
        Task SendToGroupAsync(string group, NotificationResponseDTO notificationResponseDTO);
        Task SendToUserAsync(string userId, NotificationResponseDTO notificationResponseDTO);
        Task SendToAllAsync(string message);
    }
}
