using AsyncPlate.API.Models;
using AsyncPlate.Application.Common.DTOs;
using AsyncPlate.Application.DTOs.Admin;
using AsyncPlate.Application.DTOs.Authentication;
using AsyncPlate.Application.DTOs.Recipe;
using AsyncPlate.Application.Services.Implementation;
using AsyncPlate.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AsyncPlate.API.Controllers
{
    [ApiController]
    [Route("api/[controller]s")]
    [Authorize(Roles = "Admin")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;
        private readonly ICustomerService _customerService;
        private readonly IKitchenChefService _kitchenchefService;


        public AdminController(IAdminService adminService, ICustomerService customerService,IKitchenChefService kitchenChefService)
        {
            _adminService = adminService;
            _customerService = customerService;
            _kitchenchefService = kitchenChefService;

        }

        [HttpPost("create-admin")]
        //[Authorize(Roles = "SuperAdmin")]
        public async Task<IActionResult> CreateAdmin([FromForm] CreateAdminRequestDTO requestDTO)
        {
            var responseDto = await _adminService.CreateAdminAsync(requestDTO);

            return Created($"/admins/{responseDto.Id}", new ApiResponse<AdminResponseDTO>(true, "Admin created successfully", responseDto));
        }


        [HttpGet("customers/{userId}")]  
        public async Task<IActionResult> GetCustomer([FromRoute] string userId)
        {
            var responseDTO = await _customerService.GetByUserIdAsync(userId);
            return Ok(new ApiResponse<SignupCustomerResponseDTO>(true, "Customer retrieved successfully", responseDTO));

        }

        [HttpGet("customers")]
        public async Task<IActionResult> GetCustomers([FromQuery]int pageNumber=1 , [FromQuery] int pageSize=10)
        {
            var pagedResult = await _customerService.GetAllAsync(pageNumber,pageSize);

            return Ok(new ApiResponse<PagedResult<SignupCustomerResponseDTO>>(true, "customers retrieved successfully", pagedResult));

        }

        [HttpGet("customers/vip")]
        public async Task<IActionResult> GetVipCustomers([FromQuery] int pageNumber=1, [FromQuery] int pageSize=10)
        {
            var responseDTOs = await _customerService.GetVipCustomersAsync(pageNumber,pageSize);

            return Ok(new ApiResponse<PagedResult<SignupCustomerResponseDTO>>(true, "vip customers retrieved successfully", responseDTOs));

        }

        [HttpDelete("customers/{userId}")]
        public async Task<IActionResult> DeleteCustomer([FromRoute]string userId)
        {
            await _customerService.DeleteByUserIdAsync(userId);
            return Ok(new ApiResponse<string>(true, "Customer deleted successfully",null));

        }

        
        [HttpGet("chefs/{userId}")]
        public async Task<IActionResult> GetChef([FromRoute] string userId)
        {
            var responseDTO = await _kitchenchefService.GetByUserIdAsync(userId);

            return Ok(new ApiResponse<SignupKitchenChefResponseDTO>(true,"Chef retrieved successfully.", responseDTO));
        }

        [HttpGet("chefs")]
        public async Task<IActionResult> GetChefs( [FromQuery] int pageNumber = 1,[FromQuery] int pageSize = 10)
        {
            var pagedResult = await _kitchenchefService.GetAllAsync(pageNumber, pageSize);

            return Ok(new ApiResponse<PagedResult<SignupKitchenChefResponseDTO>>(true, "Chefs retrieved successfully.",pagedResult));
        }

        
        [HttpDelete("chefs/{userId}")]
        public async Task<IActionResult> DeleteChef([FromRoute] string userId)
        {
            await _kitchenchefService.DeleteByUserIdAsync(userId);

            return Ok(new ApiResponse<string>(true, "Chef deleted successfully.",null));
        }
    }
}
