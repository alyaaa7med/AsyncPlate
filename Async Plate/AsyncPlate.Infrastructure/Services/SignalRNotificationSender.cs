using AsyncPlate.Core.Services.Interfaces;
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

        public async Task SendToGroupAsync(string group, string message)
        {
            await _hub.Clients.Group(group)
                .SendAsync("ReceiveNotification", message);
        }

        public async Task SendToUserAsync(string userId, string message)
        {
            //_logger.LogInformation($"Sending to user: {userId}");

            await _hub.Clients.User(userId)
                .SendAsync("ReceiveNotification", message);
        }
       
        public async Task SendToAllAsync(string message)
        {
            await _hub.Clients.All
                .SendAsync("ReceiveNotification", message);
        }
    }
    
}
