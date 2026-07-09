using AsyncPlate.Application.DTOs.Notification;
using AsyncPlate.Application.Interfaces.Services;
using AsyncPlate.Infrastructure.Hubs;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.ServiceModel.Channels;


namespace AsyncPlate.Infrastructure.Services
{
    
    public class SignalRService : IRealtimeService
    {
        private readonly IHubContext<RealtimeHub> _hub;
        private readonly ILogger<SignalRService> _logger;

        public SignalRService(IHubContext<RealtimeHub> hub, ILogger<SignalRService> logger)
        {
            _hub = hub;
            _logger = logger;
        }
        public async Task SendToUserAsync<T>(string userId, string eventName, T payload)
        {
            await _hub.Clients.User(userId).SendAsync(eventName, payload);
        }

        public async Task SendToGroupAsync<T>(string group, string eventName, T payload)
        {
            await _hub.Clients.Group(group).SendAsync(eventName, payload);
        }

        public async Task SendToAllAsync<T>(string eventName, T payload)
        {
            await _hub.Clients.All.SendAsync(eventName, payload);
        }

        //public async Task SendToGroupAsync(string group,  NotificationResponseDTO notificationResponseDTO )
        //{
        //    await _hub.Clients.Group(group).SendAsync("ReceiveNotification", notificationResponseDTO);
        //}

        //public async Task SendToUserAsync(string userId, NotificationResponseDTO notificationResponseDTO)
        //{
        //    //_logger.LogInformation($"Sending to user: {userId}");

        //    await _hub.Clients.User(userId).SendAsync("ReceiveNotification", notificationResponseDTO); 
        //}

        //public async Task SendToAllAsync(string message)
        //{
        //    await _hub.Clients.All.SendAsync("ReceiveNotification", message);
        //}


    }
    
}
