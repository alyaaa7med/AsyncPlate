using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Core.Services.Interfaces
{
    public interface INotificationSender
    {
        Task SendToGroupAsync(string group, string message);
        Task SendToUserAsync(string userId, string message);
    }
}
