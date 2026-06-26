using AsyncPlate.Application.Common.DTOs;
using AsyncPlate.Application.Common.Extenstions;
using AsyncPlate.Application.DTOs.Authentication;
using AsyncPlate.Application.Interfaces;
using AsyncPlate.Application.Interfaces.Services;
using AsyncPlate.Application.Services.Interfaces;
using AsyncPlate.Domain.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Application.Services.Implementation
{
    public class KitchenChefService : IKitchenChefService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IMediaService _mediaservice;
        private readonly ILogger<CustomerService> _logger;
        public KitchenChefService(IUnitOfWork unitOfWork, UserManager<AppUser> userManager,
                    IMapper mapper, ILogger<CustomerService> logger, IMediaService mediaService)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _mapper = mapper;
            _logger = logger;
            _mediaservice = mediaService;
        }
        public async Task<PagedResult<SignupKitchenChefResponseDTO>> GetAllAsync(int pageNumber, int pageSize)
        {
            var chefsQuery = _unitOfWork.kitchenChefs.GetAllWithUsers();

            var pagedResult = await chefsQuery.ToPagedResultAsync(pageNumber, pageSize);

            var responseDTOs = _mapper.Map<IEnumerable<SignupKitchenChefResponseDTO>>(pagedResult.Items);

            _logger.LogInformation(
                "Retrieved {Count} chefs (Page {PageNumber} of {TotalPages})",
                responseDTOs.Count(),
                pageNumber,
                pagedResult.TotalPages);

            return new PagedResult<SignupKitchenChefResponseDTO>
            {
                Items = responseDTOs,
                TotalCount = pagedResult.TotalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = pagedResult.TotalPages
            };
        }

        public async Task<SignupKitchenChefResponseDTO> GetByUserIdAsync(string userId)
        {
            _logger.LogInformation("Retrieving chef with id {ChefId}.", userId);

            var chef = await _unitOfWork.kitchenChefs.GetWithUserByUserIdAsync(userId);

            if (chef == null)
            {
                _logger.LogWarning("Chef with id {ChefId} was not found.", userId);

                throw new Exceptions.NotFoundException("Chef not found.");
            }

            _logger.LogInformation("Chef with id {ChefId} retrieved successfully.", userId);

            return _mapper.Map<SignupKitchenChefResponseDTO>(chef);
        }

        public async Task DeleteByUserIdAsync(string userId)
        {
            var chef = await _unitOfWork.kitchenChefs.GetWithUserByUserIdAsync(userId);

            if (chef == null)
            {
                throw new Exceptions.NotFoundException("Chef not found.");
            }

            var tokens = await _unitOfWork.onetimetokens.GetAllTokenByUserIdAsync(userId);

            await _unitOfWork.BeginTransactionAsync();

            try
            {
                // Delete behavior is Restrict, so delete children before parent.

                var profilePictureUrl = chef.AppUser.ProfilePictureUrl;

                _unitOfWork.onetimetokens.RemoveRange(tokens);

                _unitOfWork.kitchenChefs.Delete(chef);

                await _unitOfWork.SaveChangesAsync();

                var result = await _userManager.DeleteAsync(chef.AppUser);

                if (!result.Succeeded)
                {
                    var errors = string.Join(", ", result.Errors.Select(e => e.Description));

                    throw new Exceptions.BadRequestException($"Failed to delete user: {errors}");
                }

                await _unitOfWork.CommitTransactionAsync();

                if (!string.IsNullOrEmpty(profilePictureUrl))
                {
                    _mediaservice.DeleteImage(profilePictureUrl);
                }

                _logger.LogInformation("Chef deleted successfully. UserId: {UserId}", userId);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollBackTransactionAsync();

                _logger.LogError(ex, "Error deleting chef. UserId: {UserId}", userId);

                throw;
            }
        }

      
    }
}
