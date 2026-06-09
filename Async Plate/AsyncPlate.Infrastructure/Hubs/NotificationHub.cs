using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace AsyncPlate.Infrastructure.Hubs
{
    public class NotificationHub : Hub
    {

        //what will happen after connection is established with the client
        //add to groups 
        private readonly ILogger<NotificationHub> _logger;
        public NotificationHub(ILogger<NotificationHub> logger)
        {
            _logger = logger;
        }
        public override async Task OnConnectedAsync()
        {
            var user = Context.User;

            var role = user?.FindFirst(ClaimTypes.Role)?.Value;


            if (role == "Customer")
                await Groups.AddToGroupAsync(Context.ConnectionId, "Customers");

            if (role == "Chef")
                await Groups.AddToGroupAsync(Context.ConnectionId, "Chefs");

            // manually add authenticated user to personal group ((but)) signalr add this automatically 

            //var userId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            //if (userId != null)
            //    await Groups.AddToGroupAsync(Context.ConnectionId, userId);



            await base.OnConnectedAsync();
        }
    }
}
