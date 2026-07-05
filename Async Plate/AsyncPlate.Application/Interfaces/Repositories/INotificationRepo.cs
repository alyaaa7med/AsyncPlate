using AsyncPlate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Application.Interfaces.Repositories
{
    public interface INotificationRepo : IBaseRepo<Notification>
    {
        Task AddRangeAsync(IEnumerable<Notification> notifications);
        //Task<Notification> MarkAsRead(string notificationId); no need for that , i will set the property in the service 
        IQueryable<Notification> GetAllByUserId(string userId);
    }


}
