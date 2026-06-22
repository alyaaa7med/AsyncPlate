using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Application.Interfaces.Jobs
{
    public interface IOrderJob
    {
        Task SendNewOrderNotificationAsync(string orderId);
        Task SendCookingOrderNotificationAsync(string orderId);
        Task SendCompleteOrderNotificationAsync(string orderId);



    }
}
