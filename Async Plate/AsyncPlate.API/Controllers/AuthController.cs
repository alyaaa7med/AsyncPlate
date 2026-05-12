using AsyncPlate.Core.Entities;
using AsyncPlate.Core.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using AsyncPlate.API.Models;
using AsyncPlate.Core.DTOs.Authentication;

namespace AsyncPlate.API.Controllers
{

    [ApiController]
    [Route("api/[controller]")]

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

        //[HttpPost("send-email")]
        //public async Task<IActionResult> SendEmailAsync()
        //{
        //    await _authService.SendEmailAsync();

        //    return Ok(new ApiResponse<string>(true, "Email sent successfully", null));
        //}


    }
}
