using AsyncPlate.API.Models;
using AsyncPlate.Core.DTOs.Admin;
using AsyncPlate.Core.DTOs.Authentication;
using AsyncPlate.Core.Services.Implementation;
using AsyncPlate.Core.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AsyncPlate.API.Controllers
{
    [ApiController]
    [Route("api/[controller]s")]
    //[Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpPost("create-admin")]
        //[Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> CreateAdmin([FromForm] CreateAdminRequestDTO requestDTO)
        {
            var responseDto = await _adminService.CreateAdminAsync(requestDTO);


            return Created($"/admins/{responseDto.Id}", new ApiResponse<AdminResponseDTO>(true, "Admin created successfully", responseDto));
        }

    }
}
