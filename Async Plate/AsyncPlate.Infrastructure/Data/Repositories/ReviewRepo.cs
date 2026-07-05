using AsyncPlate.Application.DTOs.Review;
using AsyncPlate.Application.Interfaces.Repositories;
using AsyncPlate.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Infrastructure.Data.Repositories
{
    public class ReviewRepo: GenericRepo<Review>,IReviewRepo
    {
        public ReviewRepo(AppDbContext context) : base(context)
        {
            // create / update / get / delete are in the generic repo no need to re-implement them here
        }
        public async Task<Review?> GetReviewByOrderIdAsync(string orderId)
        {
            {
                return await _context.Reviews
                                     .Include(r=>r.Order)
                                     .FirstOrDefaultAsync(r => r.OrderId == orderId);
            }
        }
    }
}
