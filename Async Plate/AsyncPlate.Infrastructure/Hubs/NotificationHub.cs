using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;
using System.Text.RegularExpressions;

namespace AsyncPlate.Infrastructure.Hubs
{
    public class NotificationHub : Hub
    {

        //what will happen after connection is established with the client
        //add to groups 
        public override async Task OnConnectedAsync()
        {
            var user = Context.User;

            var role = user?.FindFirst(ClaimTypes.Role)?.Value;
            var userId = user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            //the user with a role will be added to 2 groups (global per role + personal group)

            //if (role == "Customer")
            //    await Groups.AddToGroupAsync(Context.ConnectionId, "Customers");

            //if (role == "Chef")
            //    await Groups.AddToGroupAsync(Context.ConnectionId, "Chefs");

            if (userId != null)
                await Groups.AddToGroupAsync(Context.ConnectionId, userId);

            await base.OnConnectedAsync();
        }
    }
}
