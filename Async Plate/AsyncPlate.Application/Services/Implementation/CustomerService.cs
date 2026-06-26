using AsyncPlate.Application.Common.DTOs;
using AsyncPlate.Application.Common.Extenstions;
using AsyncPlate.Application.DTOs.Authentication;
using AsyncPlate.Application.DTOs.Inventory;
using AsyncPlate.Application.Exceptions;
using AsyncPlate.Application.Interfaces;
using AsyncPlate.Application.Interfaces.Services;
using AsyncPlate.Application.Services.Interfaces;
using AsyncPlate.Domain.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Application.Services.Implementation
{
    public class CustomerService : ICustomerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IMediaService _mediaservice;
        private readonly ILogger<CustomerService> _logger;
        public CustomerService(IUnitOfWork unitOfWork, UserManager<AppUser> userManager,
                    IMapper mapper, ILogger<CustomerService> logger, IMediaService mediaService)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _mapper = mapper;
            _logger = logger;
            _mediaservice = mediaService;
        }

        public async Task<PagedResult<SignupCustomerResponseDTO>> GetAllAsync(int pageNumber, int pageSize) 
        {

            var customersquery = _unitOfWork.customers.GetAllWithUsers();

            var pagedResult = await customersquery.ToPagedResultAsync(pageNumber, pageSize);
          

            var responseDTOs = _mapper.Map<IEnumerable<SignupCustomerResponseDTO>>(pagedResult.Items);
            _logger.LogInformation("Retrieved {Count} inventories (Page {PageNumber} of {TotalPages})", responseDTOs.Count(), pageNumber, pagedResult.TotalPages);

            return new PagedResult<SignupCustomerResponseDTO>
            {
                Items = responseDTOs,
                TotalCount = pagedResult.TotalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = pagedResult.TotalPages
            };


        }
        public async Task<PagedResult<SignupCustomerResponseDTO>> GetVipCustomersAsync(int pageNumber, int pageSize)
        {

            var vipcustomersquery =  _unitOfWork.customers.GetVipCustomers();

            var pagedResult = await vipcustomersquery.ToPagedResultAsync(pageNumber, pageSize);

            var responseDTOs = _mapper.Map<IEnumerable<SignupCustomerResponseDTO>>(vipcustomersquery);

            _logger.LogInformation("Retrieved vip customers");


            return new PagedResult<SignupCustomerResponseDTO>
            {
                Items = responseDTOs,
                TotalCount = pagedResult.TotalCount,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalPages = pagedResult.TotalPages
            };
        }



        public async Task<SignupCustomerResponseDTO> GetByUserIdAsync(string userId)
        {
            _logger.LogInformation("Retrieving customer with id {CustomerId}.", userId);


            var customer = await _unitOfWork.customers.GetWithUserByUserIdAsync(userId);

            if (customer == null)
            {
                _logger.LogWarning("Customer with id {CustomerId} was not found.", userId);

                throw new Exceptions.NotFoundException("Customer not found.");
            }

            _logger.LogInformation("Customer with id {CustomerId} retrieved successfully.", userId);

            return _mapper.Map<SignupCustomerResponseDTO>(customer);
        }

        public async Task DeleteByUserIdAsync(string userId)
        {
            var customer = await _unitOfWork.customers.GetWithUserByUserIdAsync(userId);

            if (customer == null)
            {
                throw new Exceptions.NotFoundException("Customer not found.");
            }

            var tokens = await _unitOfWork.onetimetokens.GetAllTokenByUserIdAsync(userId);
            await _unitOfWork.BeginTransactionAsync();
            

            try
            {
                //deleting configration is restrict => so deleting child first then parent 

                var profilePictureUrl = customer.AppUser.ProfilePictureUrl;

                _unitOfWork.onetimetokens.RemoveRange(tokens);

                _unitOfWork.customers.Delete(customer);

                await _unitOfWork.SaveChangesAsync();


                var result = await _userManager.DeleteAsync(customer.AppUser);

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

                _logger.LogInformation("Customer deleted successfully. UserId: {UserId}", userId);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollBackTransactionAsync();

                _logger.LogError(ex, "Error deleting customer. UserId: {UserId}", userId);

                throw;
            }
        }
    }
}

