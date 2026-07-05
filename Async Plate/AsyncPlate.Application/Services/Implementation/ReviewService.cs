using AsyncPlate.Application.Common.DTOs;
using AsyncPlate.Application.Common.Extenstions;
using AsyncPlate.Application.DTOs.Review;
using AsyncPlate.Application.Interfaces;
using AsyncPlate.Application.Services.Interfaces;
using AsyncPlate.Domain.Entities;
using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace AsyncPlate.Application.Services.Implementation
{
    public class ReviewService : IReviewService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<AddReviewRequestDTO> _validator;
        private readonly IValidator<UpdateReviewRequestDTO> _validator2;
        private readonly ILogger<ReviewService> _logger;

        public ReviewService(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<AddReviewRequestDTO> validator,
            IValidator<UpdateReviewRequestDTO> validator2,
            ILogger<ReviewService> logger)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _validator = validator;
            _validator2 = validator2;
            _logger = logger;
        }

        public async Task<ReviewResponseDTO> AddReviewAsync(string orderId, AddReviewRequestDTO addReviewRequestDTO)
        {
            var validationResult = await _validator.ValidateAsync(addReviewRequestDTO);

            if (!validationResult.IsValid)
            {
                var errorsDictionary = validationResult.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray());

                throw new Exceptions.ValidationException(errorsDictionary);
            }

            var order = await _unitOfWork.orders.GetByIdAsync(orderId);

            if (order == null)
            {
                _logger.LogWarning("Order with id {OrderId} not found.", orderId);
                throw new Exceptions.NotFoundException("Order not found.");
            }

            var reviewExists = await _unitOfWork.reviews.GetReviewByOrderIdAsync(orderId);

            if (reviewExists != null)
            {
                _logger.LogWarning("Order with id {OrderId} has already been reviewed.", orderId);

                throw new Exceptions.ValidationException(new Dictionary<string, string[]>
                {
                    { "Review", new[] { "This order has already been reviewed." } }
                });
            }

            var review = _mapper.Map<Review>(addReviewRequestDTO);
            review.OrderId = orderId;

            _unitOfWork.reviews.Add(review);

            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Review added successfully for order {OrderId}.", orderId);

            return _mapper.Map<ReviewResponseDTO>(review);
        }

        public async Task<ReviewResponseDTO> UpdateReviewAsync(string userId,string orderId, UpdateReviewRequestDTO updateReviewRequestDTO)
        {
            var validationResult = await _validator2.ValidateAsync(updateReviewRequestDTO);

            if (!validationResult.IsValid)
            {
                var errorsDictionary = validationResult.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray());

                throw new Exceptions.ValidationException(errorsDictionary);
            }

            var review = await _unitOfWork.reviews.GetReviewByOrderIdAsync(orderId);

            if (review == null)
            {
                _logger.LogWarning("Review for order {OrderId} not found.", orderId);
                throw new Exceptions.NotFoundException("Review not found.");
            }
            //check if the user is the owner of the review
            var customer = await _unitOfWork.customers.GetWithUserByUserIdAsync(userId);
            if(customer == null)
            {
                _logger.LogWarning("Customer for user {UserId} not found.", userId);
                throw new Exceptions.NotFoundException("Customer not found.");
            }
            if (review.Order.CustomerId != customer.Id)
            {
                _logger.LogWarning("User {UserId} is not authorized to update review for order {OrderId}.", userId, orderId);
                throw new Exceptions.ForbiddenException("You are not authorized to update this review.");
            }

            _mapper.Map(updateReviewRequestDTO, review);

            _unitOfWork.reviews.Update(review);

            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Review updated successfully for order {OrderId}.", orderId);

            return _mapper.Map<ReviewResponseDTO>(review);
        }

        public async Task<ReviewResponseDTO> DeleteReviewAsync(string userId, string orderId)
        {
            var review = await _unitOfWork.reviews.GetReviewByOrderIdAsync(orderId);

            if (review == null)
            {
                _logger.LogWarning("Review for order {OrderId} not found.", orderId);
                throw new Exceptions.NotFoundException("Review not found.");
            }
            //check if the user is the owner of the review
            var customer = await _unitOfWork.customers.GetWithUserByUserIdAsync(userId);
            if(customer == null)
            {
                _logger.LogWarning("Customer for user {UserId} not found.", userId);
                throw new Exceptions.NotFoundException("Customer not found.");
            }
            if (review.Order.CustomerId != customer.Id)
            {
                _logger.LogWarning("User {UserId} is not authorized to delete review for order {OrderId}.", userId, orderId);
                throw new Exceptions.ForbiddenException("You are not authorized to delete this review.");
            }

            _unitOfWork.reviews.Delete(review);

            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Review deleted successfully for order {OrderId}.", orderId);

            return _mapper.Map<ReviewResponseDTO>(review);
        }

        public async Task<ReviewResponseDTO> GetReviewByOrderIdAsync(string orderId)
        {
            var review = await _unitOfWork.reviews.GetReviewByOrderIdAsync(orderId);

            if (review == null)
            {
                _logger.LogWarning("Review for order {OrderId} not found.", orderId);
                throw new Exceptions.NotFoundException("Review not found.");
            }

            return _mapper.Map<ReviewResponseDTO>(review);
        }

        public async Task<PagedResult<ReviewResponseDTO>> GetAllReviewsAsync(int pageNumber, int pageSize)
        {
            var reviewsQuery = _unitOfWork.reviews.GetAll();

            var pagedResult = await reviewsQuery.ToPagedResultAsync(pageNumber, pageSize);

            var responseDTOs = _mapper.Map<IEnumerable<ReviewResponseDTO>>(pagedResult.Items);

            _logger.LogInformation("Retrieved {Count} reviews.", responseDTOs.Count());

            return new PagedResult<ReviewResponseDTO>
            {
                Items = responseDTOs,
                TotalCount = pagedResult.TotalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = pagedResult.TotalPages
            };
        }
    }
}