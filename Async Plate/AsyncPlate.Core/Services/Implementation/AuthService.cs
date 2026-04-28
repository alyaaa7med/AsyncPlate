using AsyncPlate.Core.Entities;
using AsyncPlate.Core.Interfaces;
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

namespace AsyncPlate.Core.Services.Implementation
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _userRole;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IValidator<SignupCustomerRequestDTO> _validator1;
        private readonly IValidator<SignupKitchenChefRequestDTO> _validator2;
        private readonly ILogger<AuthService> _logger;
        private readonly IEmailService _emailService;

        public AuthService(
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> userRole,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IValidator<SignupCustomerRequestDTO> validator1,
            IValidator<SignupKitchenChefRequestDTO> validator2,
             ILogger<AuthService> logger,
            IEmailService emailService)
        {
            _userManager = userManager;
            _userRole = userRole;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _validator1 = validator1;
            _validator2 = validator2;
            _emailService = emailService;
        }

        public async Task<SignupCustomerResponseDTO> SignUpCustomerAsync(SignupCustomerRequestDTO requestDTO)
        {
            //check validation
            //mapper 
            //create user
            //assign role 
            //return response


            //var validationResult = await _validator1.ValidateAsync(requestDTO);

            //if (!validationResult.IsValid)
            //{
            //    var errorsDictionary = validationResult.Errors.GroupBy(e => e.PropertyName).ToDictionary(
            //            g => g.Key,
            //            g => g.ToArray().Select(e => e.ErrorMessage).ToArray()
            //        );

            //    throw new Exceptions.ValidationException(errorsDictionary); //custom validation exception
            //}

            var customer = _mapper.Map<Customer>(requestDTO);

            //var result = await _userManager.CreateAsync(customer, requestDTO.Password);
            //if (!result.Succeeded)
            //{


            //    var identityErrors = string.Join(", ", result.Errors.Select(e => e.Description));

            //    _logger.LogWarning("User creation failed: {Errors}", identityErrors);

            //    throw new BadRequestException($"Failed to create user: {identityErrors}");
            //}


            ////adding a previously created role in the database to the user
            //var roleResult = await _userManager.AddToRoleAsync(customer, "Customer");
            //if (!roleResult.Succeeded)
            //{
            //    throw new BadRequestException("Failed to assign default role to user.");
            //}
            //_logger.LogInformation("New Customer user created with ID: {UserId} ", guest.Id);
            return _mapper.Map<SignupCustomerResponseDTO>(customer);
        }


        public async Task SendEmailAsync() {    
            await _emailService.SendEmailAsync("alyaaahmed0643@gmail.com","First Email","it is a body");
        }



        public Task<SignupKitchenChefResponseDTO> SignUpKitchenChefAsync(SignupKitchenChefRequestDTO requestDTO)
        {
            return Task.FromResult(new SignupKitchenChefResponseDTO());
        }
       
        public Task SignInAsync()
        {
            return Task.CompletedTask;
        }

    }
}
