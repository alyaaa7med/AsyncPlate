using AsyncPlate.API.Models;
using AsyncPlate.Application.DTOs.Authentication;
using AsyncPlate.Application.Services.Interfaces;
using AsyncPlate.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AsyncPlate.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]

    //no need for [authorize] Because the user does NOT have identity yet or may be recovering access except logout
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }
        
        [HttpPost("signup-customer")]
        //accept the dto from body 
        //call service 
        //return the response , errors will be handled by the service which will be handled by the global error handler

        //fromform -> multipart/form-data
        //frombody -> application/json
        public async Task<IActionResult> SignUp([FromForm] SignupCustomerRequestDTO requestDTO)
        {
            var responseDto = await _authService.SignUpCustomerAsync(requestDTO);

            return Created($"/customers/{responseDto.Id}", new ApiResponse<SignupCustomerResponseDTO>(true, "Signup successful as a customer", responseDto));
        }

        [HttpPost("signup-kitchenchef")]
        //accept the dto from body 
        //call service 
        //return response , errors will be handled by the service which will be handled by the global error handler

        public async Task<IActionResult> SignUp([FromForm] SignupKitchenChefRequestDTO requestDTO)
        {
            var responseDto = await _authService.SignUpKitchenChefAsync(requestDTO);

            return Created($"/kitchenchefs/{responseDto.Id}", new ApiResponse<SignupKitchenChefResponseDTO>(true, "Signup successful as a kitchen chef", responseDto));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO requestDTO)
        {
            var responseDto = await _authService.LoginAsync(requestDTO);
            return Ok(new ApiResponse<LoginResponseDTO>(true, "Login successful", responseDto));
        }
        [HttpPost("forget-password")]
        public async Task<IActionResult> ForgetPassword([FromBody] ForgetPasswordRequestDTO requestDTO)
        {
             await _authService.ForgetPasswordAsync(requestDTO);

            return Ok(new ApiResponse<object>(true, "If an account exists, a reset link has been sent.", null));
        }
        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordRequestDTO requestDTO)
        {
            await _authService.ResetPasswordAsync(requestDTO);
            return Ok(new ApiResponse<object>(true, "Password reset successful", null));
        }

        [Authorize]
        [HttpPost("logout")]
        public async Task<IActionResult> Logout()  //no dto here , id is from the token 
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized(new ApiResponse<object>(false, "Invalid user ", null));
            }

            await _authService.LogoutAsync(userId);
            return Ok(new ApiResponse<object>(true, "Logout from all sessions successful", null));
        }
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDTO requestDTO)
        {
            var responseDto = await _authService.RefreshTokenAsync(requestDTO);
            return Ok(new ApiResponse<RefreshTokenResponseDTO>(true, "Token refreshed successfully", responseDto));
        }


        //[HttpPost("send-email")]
        //public async Task<IActionResult> SendEmailAsync()
        //{
        //    await _authService.SendEmailAsync();

        //    return Ok(new ApiResponse<string>(true, "Email sent successfully", null));
        //}


    }
}
