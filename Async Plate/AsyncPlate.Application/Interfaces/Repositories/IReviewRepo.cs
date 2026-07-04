using AsyncPlate.Application.DTOs.Review;
using AsyncPlate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Application.Interfaces.Repositories
{
    public interface IReviewRepo:IBaseRepo<Review>
    {
        Task<Review?> GetReviewByOrderIdAsync(string orderId);
    }
}
