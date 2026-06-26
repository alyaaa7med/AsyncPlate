using AsyncPlate.API.Models;
using AsyncPlate.Application.DTOs.Authentication;
using AsyncPlate.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AsyncPlate.API.Controllers
{
    [ApiController]
    [Route("api/[controller]s")]
    [Authorize(Roles = "Customer")]   
    public class CustomerController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomerController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMyAccount()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized(new ApiResponse<object>(false, "Invalid user ", null));

            var responseDTO = await _customerService.GetByUserIdAsync(userId);

            return Ok(new ApiResponse<SignupCustomerResponseDTO>(true, "your account retrieved successfully", responseDTO));
        }

        [HttpDelete("me")]
        public async Task<IActionResult> DeleteMyAccount()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (userId == null)
                return Unauthorized(new ApiResponse<object>(false, "Invalid user ", null));

            await _customerService.DeleteByUserIdAsync(userId);

            return Ok(new ApiResponse<string>(true, "your account deleted successfully", null));
        }
    }

}
