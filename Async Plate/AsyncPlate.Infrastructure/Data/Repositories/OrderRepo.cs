using AsyncPlate.Application.Interfaces.Repositories;
using AsyncPlate.Application.DTOs.Order;
using AsyncPlate.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Infrastructure.Data.Repositories
{
    public class OrderRepo : GenericRepo<Order>, IOrderRepo
    {
        public OrderRepo(AppDbContext context) : base(context)
        {
        }

        public async Task<Order?> GetOrderWithOrderItemsAndExtraOrderItemsByIdAsync(string orderId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)

                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Extras)
                        .ThenInclude(e => e.Product)

                .FirstOrDefaultAsync(o => o.Id == orderId);
        }
    }
}
