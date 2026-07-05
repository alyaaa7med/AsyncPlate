using AsyncPlate.API.Models;
using AsyncPlate.Application.Common.DTOs;
using AsyncPlate.Application.DTOs.Product;
using AsyncPlate.Application.DTOs.Review;
using AsyncPlate.Application.Services.Implementation;
using AsyncPlate.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AsyncPlate.API.Controllers
{
    [ApiController]
    [Route("api/orders")]
    [Authorize("Customer")]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewService _reviewService;

        public ReviewController(IReviewService reviewService)
        {
            _reviewService = reviewService;
        }

        [HttpPost("{orderId}/review")]
        public async Task<IActionResult> AddReview([FromRoute] string orderId, [FromBody] AddReviewRequestDTO addReviewRequestDTO)
        {
            var responseDto = await _reviewService.AddReviewAsync(orderId, addReviewRequestDTO);
            return Created($"/reviews/{responseDto.Id}", new ApiResponse<ReviewResponseDTO>(true, "review added successfully", responseDto));
        }

        [HttpPut("{orderId}/review")]
        public async Task<IActionResult> UpdateReview([FromRoute] string orderId, [FromBody] UpdateReviewRequestDTO updateReviewRequestDTO)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
                return Unauthorized(new ApiResponse<object>(false, "Invalid user.", null));

            var responseDto = await _reviewService.UpdateReviewAsync(userId, orderId, updateReviewRequestDTO);
            return Created($"/reviews/{responseDto.Id}", new ApiResponse<ReviewResponseDTO>(true, "review updated successfully", responseDto));
        }


        [HttpGet("{orderId}/review")]
        public async Task<IActionResult> GetReviewByOrderId([FromRoute] string orderId)
        {
            var reviewResponseDTO =
                await _reviewService.GetReviewByOrderIdAsync(orderId);

            return Ok(new ApiResponse<ReviewResponseDTO>( true, "Review retrieved successfully.",reviewResponseDTO));
        }

        [HttpDelete("{orderId}/review")]
        public async Task<IActionResult> DeleteReview([FromRoute] string orderId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
                return Unauthorized(new ApiResponse<object>(false, "Invalid user.", null));

            var reviewResponseDTO = await _reviewService.DeleteReviewAsync(userId, orderId);

            return Ok(new ApiResponse<ReviewResponseDTO>(true,"Review deleted successfully.", reviewResponseDTO));
        }

        [HttpGet("reviews")]
        public async Task<IActionResult> GetAllReviews([FromQuery] int pageNumber = 1,[FromQuery] int pageSize = 10)
        {
            var pagedResult = await _reviewService.GetAllReviewsAsync(pageNumber, pageSize);

            return Ok(new ApiResponse<PagedResult<ReviewResponseDTO>>(true,"Reviews retrieved successfully.",pagedResult));
        }


    }

    }
