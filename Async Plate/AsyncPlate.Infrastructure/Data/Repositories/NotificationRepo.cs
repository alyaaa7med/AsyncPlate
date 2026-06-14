using AsyncPlate.Application.Interfaces.Repositories;
using AsyncPlate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Infrastructure.Data.Repositories
{
    public class NotificationRepo : GenericRepo<Notification>, INotificationRepo
    {
        public NotificationRepo(AppDbContext context) : base(context)
        {
        }

        public async Task AddRangeAsync(IEnumerable<Notification> notifications)
        {
            await _context.Set<Notification>().AddRangeAsync(notifications);
        }

        
    }
}
