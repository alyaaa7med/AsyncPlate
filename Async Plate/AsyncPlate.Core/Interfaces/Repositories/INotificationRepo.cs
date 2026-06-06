using AsyncPlate.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Core.Interfaces.Repositories
{
    public interface INotificationRepo : IBaseRepo<Notification>
    {
        Task AddRangeAsync(IEnumerable<Notification> notifications);
    }
}
