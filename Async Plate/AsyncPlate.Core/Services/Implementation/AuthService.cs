using AsyncPlate.Core.DTOs.Authentication;
using AsyncPlate.Core.Entities;
using AsyncPlate.Core.Exceptions;
using AsyncPlate.Core.Interfaces;
using AsyncPlate.Core.Interfaces.Services;
using AsyncPlate.Core.Services.Interfaces;
using AutoMapper;
using FluentValidation;
using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities; 
using Microsoft.Extensions.Logging;
using System.Text;
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
        private readonly IValidator<ForgetPasswordRequestDTO> _validator4;
        private readonly IValidator<ResetPasswordRequestDTO> _validator5;
        private readonly IValidator<RefreshTokenRequestDTO> _validator6;
        private readonly IEmailJobService _emailService;
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
            IValidator<ForgetPasswordRequestDTO> validator4,
            IValidator<ResetPasswordRequestDTO> validator5,
            IValidator<RefreshTokenRequestDTO> validator6,
            IEmailJobService emailService,
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
            _validator4 = validator4;
            _validator5 = validator5;
            _validator6 = validator6;
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


            var customer = _mapper.Map<Customer>(requestDTO);

            
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
                    var identityErrors = string.Join(", ", result.Errors.Select(e => e.Description));
                    _logger.LogWarning("User creation failed: {Errors}", identityErrors);
                    throw new Exceptions.BadRequestException($"Failed to create user: {identityErrors}");//badrequest : email duplicate / weak password 
                }

                await _unitOfWork.customers.AddAsync(customer);//nav prop of user is already filled
                                                               //so that no need to add fk manully to link customer 
                                                               //with user

                var roleResult = await _userManager.AddToRoleAsync(customer.AppUser, "Customer");//just one role

                if (!roleResult.Succeeded)
                {
                    var roleErrors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                    throw new Exceptions.InternalServerException($"Failed to assign role: {roleErrors}");
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
                    var identityErrors = string.Join(", ", result.Errors.Select(e => e.Description));
                    _logger.LogWarning("User creation failed: {Errors}", identityErrors);
                    throw new Exceptions.BadRequestException($"Failed to create user: {identityErrors}");
                }

                await _unitOfWork.kitchenChefs.AddAsync(chef);//nav prop of user is already filled
                                                              //so that no need to add fk manully to link chef 
                                                              //with user

                var roleResult = await _userManager.AddToRoleAsync(chef.AppUser, "Chef");

                if (!roleResult.Succeeded)
                {
                    var roleErrors = string.Join(", ", roleResult.Errors.Select(e => e.Description));
                    throw new Exceptions.InternalServerException($"Failed to assign role: {roleErrors}");
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
                throw new Exceptions.UnauthorizedException("Invalid email or password.");
            }
            var passwordValid = await _userManager.CheckPasswordAsync(user, requestDTO.Password);//how this works? it hashes the provided password and compares it to the stored hash in the database, returning true if they match and false otherwise.
            if (!passwordValid)
            {
                _logger.LogWarning("Failed login attempt: Invalid password for email {Email}.", requestDTO.Email);
                throw new Exceptions.UnauthorizedException("Invalid email or password.");
            }

            //how to get the type and generate token? 
            var accessToken = _tokenService.CreateAccessToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken();

            //user.RefreshTokens.Add(new RefreshToken { RefreshTokenValue = refreshToken, UserId= user.Id});
            //await _userManager.UpdateAsync(user);  أنت بتضيف ريفريش جوا توكن لكن المنجر مش هيعرف يحفظ الا يوزر فقط

            await _unitOfWork.refreshtokens.AddAsync(new RefreshToken { RefreshTokenValue = refreshToken, UserId = user.Id }); //need to link userid fk here as i do not have nav prop to user in refresh token to link them directly
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation( "User logged in successfully. UserId: {UserId}", user.Id);
            var responsedto = _mapper.Map<LoginResponseDTO>(user);
            responsedto.AccessToken = accessToken;
            responsedto.RefreshToken = refreshToken;
            return responsedto;
        }


        public async Task ForgetPasswordAsync(ForgetPasswordRequestDTO requestDTO)
        {
            //validate dto 
            //check if email not found 
            //invalidate and expire old token for the user to prevent reuse of old tokens if any
            //create new token 
            //send email 
            var validationResult = await _validator4.ValidateAsync(requestDTO);

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
                _logger.LogWarning("Failed find user: User with email {Email} not found.", requestDTO.Email);
                //not efficient//throw new Exceptions.NotFoundException("If the email exists, a reset link has been sent.");
                return;
            }

          
            //inactivate old token for the user to prevent reuse of old tokens if any
            var token = await _unitOfWork.onetimetokens.GetActiveOneTimeTokenAsync(t => t.AppUserId == user.Id && t.IsActive);

            if (token != null)
            {
                token.IsActive = false;
                token.ExpiryDate = DateTime.UtcNow;
                _unitOfWork.onetimetokens.Update(token);

            }
            //generate new token and save it to db
            var ott = new OneTimeToken
            {
                Token = Guid.NewGuid().ToString(),
                AppUserId = user.Id
            };//i need to add the fk as i do not have nav, to link the 2 entities 

            await _unitOfWork.onetimetokens.AddAsync(ott);
            await _unitOfWork.SaveChangesAsync();
            //new onetimetoken added for user with email 

            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(ott.Token));
            var resetLink = $"https://yourfrontend.com/reset-password?token={encodedToken}&email={user.Email}"; //to do in appsettings



            BackgroundJob.Enqueue<IEmailJobService>(x => x.SendEmailAsync(
            user.Email!,
           "Reset Password",
           $"Click here to reset your password: {resetLink}"
       )
   );
            _logger.LogInformation("Forget Password Email is sent to user with email {email}", user.Email);

        }



        public async Task ResetPasswordAsync(ResetPasswordRequestDTO requestDTO)
        {
            //validate dto 
            //check if email not found
            //check if ott is valid 
            //reset password
            //invalid the token to prevent reuse
            //force logout 
            //return dto
            var validationResult = await _validator5.ValidateAsync(requestDTO);

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
                _logger.LogWarning("Failed find user: User with email {Email} not found.", requestDTO.Email);
                throw new Exceptions.NotFoundException("User", requestDTO.Email);
            }
            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(requestDTO.OneTimeToken));

            var ott = await _unitOfWork.onetimetokens.GetActiveOneTimeTokenAsync(t => t.Token == decodedToken
                                                                && t.AppUserId == user.Id
                                                                && t.ExpiryDate > DateTime.UtcNow
                                                                && t.IsActive == true);

            if (ott == null)
            {
                _logger.LogWarning("Invalid or expired one-time token for email {Email}.", requestDTO.Email);
                throw new Exceptions.UnauthorizedException("Invalid or expired token.");
            }
            await _unitOfWork.BeginTransactionAsync(); 
            try
            {

                var result = await _userManager.RemovePasswordAsync(user);
                if (!result.Succeeded)
                {
                    throw new InternalServerException("Failed to reset password.");
                }
                var addResult = await _userManager.AddPasswordAsync(user, requestDTO.NewPassword);
                if (!addResult.Succeeded)
                {
                    throw new InternalServerException("Failed to set new password.");
                }
                ott.IsActive = false;
                ott.ExpiryDate = DateTime.UtcNow;
                _unitOfWork.onetimetokens.Update(ott);

                await _unitOfWork.SaveChangesAsync();

                await _unitOfWork.CommitTransactionAsync();
            }
            catch
            {
                await _unitOfWork.RollBackTransactionAsync();
                throw;
            }
           //force logout 
            await LogoutAsync(user.Id);

            _logger.LogInformation("User with email {email} is forced to logout after password reset", user.Email);
        }

        public async Task LogoutAsync (string userId)
        {
            //revoke all active tokens for the user to force logout from all sessions
            var activeTokens = await _unitOfWork.refreshtokens.FindActiveTokensByUserIdAsync(userId);
            foreach (var token in activeTokens) 
            {
                token.IsRevoked = true;
                token.ExpiredAt = DateTime.UtcNow; 
                token.IsExpired = true;
                _unitOfWork.refreshtokens.Update(token);
            }
            await _unitOfWork.SaveChangesAsync(); //save all not one by one to optimize performance as dbcontext is shared

            _logger.LogInformation("User with ID {UserId} has been logged out and all active tokens revoked.", userId);
        }

        public async Task<RefreshTokenResponseDTO> RefreshTokenAsync(RefreshTokenRequestDTO requestDTO)
        {
            //validate dto 
            //check if token is valid 
            //generate new access token and refresh token
            //revoke old refresh token 
            //return new tokens
            var validationResult = await _validator6.ValidateAsync(requestDTO);
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

            var existingToken = await _unitOfWork.refreshtokens.CheckRefreshTokenAsync( requestDTO.RefreshToken);
            if (existingToken == null)
            {
                _logger.LogWarning("Invalid refresh token: {RefreshToken}", requestDTO.RefreshToken);
                throw new Exceptions.UnauthorizedException("Invalid authentication.");
            }
            var user = await _userManager.FindByIdAsync(existingToken.UserId);
            if (user == null)
            {
                _logger.LogWarning("User not found for refresh token: {RefreshToken}", requestDTO.RefreshToken);
                throw new Exceptions.UnauthorizedException("Invalid authentication.");
            }
            var newAccessToken = _tokenService.CreateAccessToken(user);
            var newRefreshToken = _tokenService.GenerateRefreshToken();
                        
            existingToken.IsRevoked = true;
            existingToken.IsExpired = true;
            existingToken.ExpiredAt = DateTime.UtcNow;
            _unitOfWork.refreshtokens.Update(existingToken);

            await _unitOfWork.refreshtokens.AddAsync(new RefreshToken
            {
                RefreshTokenValue = newRefreshToken,
                UserId = user.Id
            });

            await _unitOfWork.SaveChangesAsync(); //save once to optimize performance as dbcontext is shared
            
            return new RefreshTokenResponseDTO
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshToken
            };
        }

        




    }
}
