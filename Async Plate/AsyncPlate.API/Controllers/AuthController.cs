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

        [HttpPost("signup-guest")]
        //accept the dto from body 
        //call service 
        //return the dtos , errors will be handled by the service which will be handled by the global error handler

        public async Task<IActionResult> SignUp([FromBody] SignupCustomerRequestDTO requestDTO)
        {
            var responseDto = await _authService.SignUpCustomerAsync(requestDTO);

            return Created($"/customers/{responseDto.Id}", new ApiResponse<SignupCustomerResponseDTO>(true, "Signup successful", responseDto));
        }
        [HttpPost("send-email")]
        public async Task<IActionResult> SendEmailAsync()
        {
            await _authService.SendEmailAsync();

            return Ok(new ApiResponse<string>(true, "Email sent successfully", null));
        }
        

    }
}
