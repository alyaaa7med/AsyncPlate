using AsyncPlate.Application.Common.DTOs;
using AsyncPlate.Application.DTOs.Review;
using AsyncPlate.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Application.Services.Interfaces
{
    public interface IReviewService
    {
        Task<ReviewResponseDTO> AddReviewAsync(string orderId, AddReviewRequestDTO addReviewRequestDTO);
        Task<ReviewResponseDTO> UpdateReviewAsync(string userId,string reviewId, UpdateReviewRequestDTO updateReviewRequestDTO);
        Task<ReviewResponseDTO> GetReviewByOrderIdAsync(string orderId);
        Task<ReviewResponseDTO> DeleteReviewAsync(string userId,string orderId);
        Task<PagedResult<ReviewResponseDTO>> GetAllReviewsAsync(int pageNumber, int pageSize);

    }
}
