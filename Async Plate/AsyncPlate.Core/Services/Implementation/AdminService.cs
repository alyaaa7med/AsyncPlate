using AsyncPlate.Core.DTOs.Admin;
using AsyncPlate.Core.DTOs.Authentication;
using AsyncPlate.Core.Entities;
using AsyncPlate.Core.Interfaces;
using AsyncPlate.Core.Interfaces.Services;
using AsyncPlate.Core.Services.Interfaces;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Core.Services.Implementation
{
    public class AdminService : IAdminService
    {


        private readonly ILogger<AdminService> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<CreateAdminRequestDTO> _validator1;
        private readonly IEmailJobService _emailService;
        private readonly IMediaService _mediaService;
        private readonly UserManager<AppUser> _userManager;
        public AdminService(
            IMapper mapper,
            ILogger<AdminService> logger,
            IUnitOfWork unitOfWork,
            IValidator<CreateAdminRequestDTO> validator1,
            IEmailJobService emailService,
            IMediaService mediaService,
            UserManager<AppUser> userManager)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _validator1 = validator1;
            _emailService = emailService;
            _mediaService = mediaService;

        }

        public async Task<AdminResponseDTO> CreateAdminAsync(CreateAdminRequestDTO requestDTO)
        {
            //check validation
            //mapper 
            //create user
            //create customer 
            //assign role  
            //return dto


            var validationResult = await _validator1.ValidateAsync(requestDTO);

            if (!validationResult.IsValid)
            {
                var errorsDictionary = validationResult.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray()
                    );

                throw new Exceptions.ValidationException(errorsDictionary);
            }


            var admin = _mapper.Map<Admin>(requestDTO);


            await _unitOfWork.BeginTransactionAsync();
            try
            {
                if (requestDTO.AppUser.ProfilePicture != null)
                {
                    var imageUrl = await _mediaService.UploadImageAsync(requestDTO.AppUser.ProfilePicture, "admins");
                    admin.AppUser.ProfilePictureUrl = imageUrl;
                }
                // 3) Create Identity user
                var result = await _userManager.CreateAsync(
                    admin.AppUser,
                    requestDTO.AppUser.Password
                );

                if (!result.Succeeded)
                {
                    var identityErrors = string.Join(", ", result.Errors.Select(e => e.Description));
                    _logger.LogWarning("User creation failed: {Errors}", identityErrors);
                    throw new Exceptions.BadRequestException($"Failed to create user: {identityErrors}");//badrequest : email duplicate / weak password 
                }

                await _unitOfWork.admins.AddAsync(admin);//nav prop of user is already filled
                                                         //so that no need to add fk manully to link customer 
                                                         //with user

                var roleResult = await _userManager.AddToRoleAsync(admin.AppUser, "Admin");//just one role

                if (!roleResult.Succeeded)
                {
                    var roleErrors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                    throw new Exceptions.InternalServerException($"Failed to assign role: {roleErrors}");
                }

                await _unitOfWork.CommitTransactionAsync();
                _logger.LogInformation("New Admin user created with UserID: {UserId}", admin.AppUser.Id);
                return _mapper.Map<AdminResponseDTO>(admin);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollBackTransactionAsync();
                _logger.LogError(ex, "Unexpected error during registration for email: {Email}", requestDTO.AppUser.Email);

                if (!string.IsNullOrEmpty(admin.AppUser.ProfilePictureUrl))
                {
                    _mediaService.DeleteImage(admin.AppUser.ProfilePictureUrl);
                }

                throw;
            }
        }


    }
}

