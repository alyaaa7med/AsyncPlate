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
    [Authorize(Roles = "KitchenChef")]
    public class ChefController : ControllerBase
    {
        private readonly IKitchenChefService _kitchenchefService;

        public ChefController(IKitchenChefService chefService)
        {
            _kitchenchefService = chefService;
        }

        [HttpGet("me")]
        public async Task<IActionResult> GetMyAccount()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
                return Unauthorized(new ApiResponse<object>(false, "Invalid user.", null));

            var responseDTO = await _kitchenchefService.GetByUserIdAsync(userId);

            return Ok(new ApiResponse<SignupKitchenChefResponseDTO>(true,"Your account retrieved successfully.", responseDTO));
        }


        [HttpDelete("me")]
        public async Task<IActionResult> DeleteMyAccount()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            if (userId == null)
                return Unauthorized(new ApiResponse<object>(false, "Invalid user.", null));

            await _kitchenchefService.DeleteByUserIdAsync(userId);

            return Ok(new ApiResponse<string>(true, "Your account deleted successfully.", null));
        }

    }
}
