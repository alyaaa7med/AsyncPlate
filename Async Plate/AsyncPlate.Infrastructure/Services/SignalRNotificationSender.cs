using AsyncPlate.Application.DTOs.Notification;
using AsyncPlate.Application.Interfaces.Services;
using AsyncPlate.Infrastructure.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.ServiceModel.Channels;


namespace AsyncPlate.Infrastructure.Services
{
    
    public class SignalRNotificationSender : INotificationSender
    {
        private readonly IHubContext<NotificationHub> _hub;
        private readonly ILogger<SignalRNotificationSender> _logger;

        public SignalRNotificationSender(IHubContext<NotificationHub> hub, ILogger<SignalRNotificationSender> logger)
        {
            _hub = hub;
            _logger = logger;
        }

        public async Task SendToGroupAsync(string group,  NotificationResponseDTO notificationResponseDTO )
        {
            await _hub.Clients.Group(group).SendAsync("ReceiveNotification", notificationResponseDTO);
        }

        public async Task SendToUserAsync(string userId, NotificationResponseDTO notificationResponseDTO)
        {
            //_logger.LogInformation($"Sending to user: {userId}");

            await _hub.Clients.User(userId).SendAsync("ReceiveNotification", notificationResponseDTO); 
        }
       
        public async Task SendToAllAsync(string message)
        {
            await _hub.Clients.All.SendAsync("ReceiveNotification", message);
        }

        
    }
    
}
