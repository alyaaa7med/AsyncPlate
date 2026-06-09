using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Application.Interfaces.Services
{
    public interface INotificationSender
    {
        Task SendToGroupAsync(string group, string message);
        Task SendToUserAsync(string userId, string message);
        Task SendToAllAsync(string message);
    }
}
