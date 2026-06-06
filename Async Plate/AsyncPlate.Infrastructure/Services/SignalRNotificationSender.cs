using AsyncPlate.Core.Services.Interfaces;
using AsyncPlate.Infrastructure.Hubs;
using Microsoft.AspNetCore.SignalR;


namespace AsyncPlate.Infrastructure.Services
{
    
    public class SignalRNotificationSender : INotificationSender
    {
        private readonly IHubContext<NotificationHub> _hub;

        public SignalRNotificationSender(IHubContext<NotificationHub> hub)
        {
            _hub = hub;
        }

        public async Task SendToGroupAsync(string group, string message)
        {
            await _hub.Clients.Group(group)
                .SendAsync("ReceiveNotification", message);
        }

        public async Task SendToUserAsync(string userId, string message)
        {
            await _hub.Clients.User(userId)
                .SendAsync("ReceiveNotification", message);
        }
    }
}
