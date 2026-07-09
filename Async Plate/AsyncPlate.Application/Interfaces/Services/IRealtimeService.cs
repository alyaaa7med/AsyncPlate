using AsyncPlate.Application.DTOs.Notification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Application.Interfaces.Services
{
    public interface IRealtimeService
    {
        //Task SendToGroupAsync(string group, NotificationResponseDTO notificationResponseDTO);
        //Task SendToUserAsync(string userId, NotificationResponseDTO notificationResponseDTO);
        //Task SendToAllAsync(string message);
        
            Task SendToUserAsync<T>(string userId, string eventName, T payload);

            Task SendToGroupAsync<T>(string group, string eventName, T payload);

            Task SendToAllAsync<T>(string eventName, T payload);
        
    }
}
