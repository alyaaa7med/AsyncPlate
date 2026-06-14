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


        public async Task<int> GetTodayOrdersCountAsync()
        {
            var today = DateTime.UtcNow.Date;

            return await _context.Orders
                .Where(o => o.CreatedAt.Date == today)
                .CountAsync();
        }
        public async Task<int> GetCompletedOrdersCountAsync()
        {
            var today = DateTime.UtcNow.Date;

            return await _context.Orders
                .Where(o => o.CreatedAt.Date == today && o.Status == OrderStatus.Completed)
                .CountAsync();
        }
        public async Task<int> GetCancelledOrdersCountAsync()
        {
            var today = DateTime.UtcNow.Date;

            return await _context.Orders
                .Where(o => o.CreatedAt.Date == today && o.Status == OrderStatus.Cancelled)
                .CountAsync();
        }

        public async Task<decimal> GetTodayRevenueAsync()
        {
            var today = DateTime.UtcNow.Date;

            return await _context.Orders
                .Where(o => o.CreatedAt.Date == today && o.Status == OrderStatus.Completed)
                .SumAsync(o => o.TotalFeeTotal);
        }

    }
}
/*Hangfire Job
   ↓
Generate Report (DTO)
   ↓
Generate PDF (QuestPDF)
   ↓
EmailService (MailKit)
   ↓
Admin receives email with attachment */