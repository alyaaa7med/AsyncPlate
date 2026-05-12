using AsyncPlate.Core.Entities;
using AsyncPlate.Core.Services.Interfaces;
using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsyncPlate.Core.Exceptions;
using AsyncPlate.Core.DTOs.Authentication;
using Microsoft.Extensions.Logging;
using AsyncPlate.Core.Interfaces.Repositories;
using AsyncPlate.Core.Interfaces.Services;
using AsyncPlate.Core.Interfaces;

namespace AsyncPlate.Core.Services.Implementation
{
    public class AuthService : IAuthService
    {

        private readonly ILogger<AuthService> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<SignupCustomerRequestDTO> _validator1;
        private readonly IValidator<SignupKitchenChefRequestDTO> _validator2;
        private readonly IValidator<LoginRequestDTO> _validator3;
        private readonly IEmailService _emailService;
        private readonly IMediaService _mediaService;
        private readonly ITokenService _tokenService;
        private readonly UserManager<AppUser> _userManager;
        public AuthService(
            IMapper mapper,
            ILogger<AuthService> logger,
            IUnitOfWork unitOfWork,
            IValidator<SignupCustomerRequestDTO> validator1,
            IValidator<SignupKitchenChefRequestDTO> validator2,
            IValidator<LoginRequestDTO> validator3,
            IEmailService emailService,
            IMediaService mediaService,
            ITokenService tokenService,
            UserManager<AppUser> userManager)
        {
            _userManager = userManager;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _validator1 = validator1;
            _validator2 = validator2;
            _validator3 = validator3;
            _emailService = emailService;
            _mediaService = mediaService;
            _tokenService = tokenService;

        }

        public async Task<SignupCustomerResponseDTO> SignUpCustomerAsync(SignupCustomerRequestDTO requestDTO)
        {
            //check validation
            //mapper 
            //create user
            //create customer 
            //assign role  
            //return response


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


            var customer = _mapper.Map<Customer>(requestDTO);

            // Safety check
            if (customer.AppUser == null)
            {
                throw new BadRequestException("Customer AppUser mapping failed.");
            }
            await _unitOfWork.BeginTransactionAsync();
            try
            {
                if (requestDTO.AppUser.ProfilePicture != null)
                {
                    var imageUrl = await _mediaService.UploadImageAsync(requestDTO.AppUser.ProfilePicture, "customers");
                    customer.AppUser.ProfilePictureUrl = imageUrl;
                }
                // 3) Create Identity user
                var result = await _userManager.CreateAsync(
                    customer.AppUser,
                    requestDTO.AppUser.Password
                );

                if (!result.Succeeded)
                {
                    await _unitOfWork.RollBackTransactionAsync();
                    var identityErrors = string.Join(", ", result.Errors.Select(e => e.Description));
                    _logger.LogWarning("User creation failed: {Errors}", identityErrors);
                    throw new BadRequestException($"Failed to create user: {identityErrors}");
                }

                await _unitOfWork.customers.AddAsync(customer);

                var roleResult = await _userManager.AddToRoleAsync(customer.AppUser, "Customer");//just one role

                if (!roleResult.Succeeded)
                {
                    await _unitOfWork.RollBackTransactionAsync();
                    var roleErrors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                    throw new BadRequestException($"Failed to assign role: {roleErrors}");
                }
                await _unitOfWork.CommitTransactionAsync();
                _logger.LogInformation("New Customer user created with UserID: {UserId}", customer.AppUser.Id);
                return _mapper.Map<SignupCustomerResponseDTO>(customer);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollBackTransactionAsync();
                _logger.LogError(ex, "Unexpected error during registration for email: {Email}", requestDTO.AppUser.Email);

                if (!string.IsNullOrEmpty(customer.AppUser.ProfilePictureUrl))
                {
                    _mediaService.DeleteImage(customer.AppUser.ProfilePictureUrl);
                }

                throw;
            }
        }
       

   

        public async Task<SignupKitchenChefResponseDTO> SignUpKitchenChefAsync(SignupKitchenChefRequestDTO requestDTO)
        {
            //validate dto 
            //mapping 2 levels 
            //create chef
            //add roles
            //mapping and return dto
            var validationResult = await _validator2.ValidateAsync(requestDTO);

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

            var chef = _mapper.Map<KitchenChef>(requestDTO);
            if (chef.AppUser == null)
            {
                throw new BadRequestException("KitchenChef AppUser mapping failed.");
            }
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                if (requestDTO.AppUser.ProfilePicture != null)
                {
                    var imageUrl = await _mediaService.UploadImageAsync(requestDTO.AppUser.ProfilePicture, "chefs");
                    chef.AppUser.ProfilePictureUrl = imageUrl;
                }

                var result = await _userManager.CreateAsync(
                    chef.AppUser,
                    requestDTO.AppUser.Password
                );

                if (!result.Succeeded)
                {
                    await _unitOfWork.RollBackTransactionAsync();
                    var identityErrors = string.Join(", ", result.Errors.Select(e => e.Description));
                    _logger.LogWarning("Chef User creation failed: {Errors}", identityErrors);
                    throw new BadRequestException($"Failed to create user: {identityErrors}");
                }

                await _unitOfWork.kitchenChefs.AddAsync(chef);

                var roleResult = await _userManager.AddToRoleAsync(chef.AppUser, "Chef");

                if (!roleResult.Succeeded)
                {
                    await _unitOfWork.RollBackTransactionAsync();
                    var roleErrors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                    throw new BadRequestException($"Failed to assign role: {roleErrors}");
                }

                await _unitOfWork.CommitTransactionAsync();
                _logger.LogInformation("New KitchenChef user created with UserID: {UserId}", chef.AppUser.Id);
                return _mapper.Map<SignupKitchenChefResponseDTO>(chef);
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollBackTransactionAsync();
                _logger.LogError(ex, "Unexpected error during Chef registration for email: {Email}", requestDTO.AppUser.Email);
                if (!string.IsNullOrEmpty(chef.AppUser.ProfilePictureUrl))
                {
                     _mediaService.DeleteImage(chef.AppUser.ProfilePictureUrl);
                }

                throw;
            }
        
        }

        public async Task<LoginResponseDTO> LoginAsync(LoginRequestDTO requestDTO)
        {
            //validate dto 
            //find user by email
            //check password
            //generate token
            //return dto
            var validationResult = await _validator3.ValidateAsync(requestDTO);

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

            var user = await _userManager.FindByEmailAsync(requestDTO.Email); //i used _userrepo but it always invalid
            if (user == null)

            {
                _logger.LogWarning("Failed login attempt: User with email {Email} not found.", requestDTO.Email);
                throw new BadRequestException("Invalid email or password.");
            }
            var passwordValid = await _userManager.CheckPasswordAsync(user, requestDTO.Password);//how this works? it hashes the provided password and compares it to the stored hash in the database, returning true if they match and false otherwise.
            if (!passwordValid)
            {
                _logger.LogWarning("Failed login attempt: Invalid password for email {Email}.", requestDTO.Email);
                throw new BadRequestException("Invalid email or password.");
            }

            //how to get the type and generate token? 
            var accessToken = _tokenService.CreateAccessToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken();

            user.RefreshTokens.Add(new RefreshToken { RefreshTokenValue = refreshToken, });
            await _userManager.UpdateAsync(user);

            var responsedto = _mapper.Map<LoginResponseDTO>(user); 
            responsedto.AccessToken = accessToken;
            responsedto.RefreshToken = refreshToken;
            return responsedto;
        }


        public async Task SendEmailAsync()
        {
            await _emailService.SendEmailAsync("alyaaahmed0643@gmail.com", "First Email", "it is a body");
        }






    }
}
