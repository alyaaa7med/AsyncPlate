using AsyncPlate.API.Models;
using AsyncPlate.Application.Common;
using AsyncPlate.Application.Common.DTOs;
using AsyncPlate.Application.DTOs.Menu;
using AsyncPlate.Application.Interfaces;
using AsyncPlate.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AsyncPlate.API.Controllers.Customer
{
    [Route("api/customer")]
    [ApiController]
    //[Authorize(Roles = "Customer")]
    public class MenuController : ControllerBase
    {
        private readonly IMenuService _menuService;


        public MenuController(IMenuService menuService)
        {
            _menuService = menuService;
        }

        [HttpGet("menu")]
        public async Task<IActionResult> GetMenu([FromQuery] MenuFilterDTO filter)
        {
            var response = await _menuService.GetMenuAsync(filter);

            return Ok(new ApiResponse<PagedResult<MenuItemResponseDTO>>(true, "Menu retrieved successfully.", response));
        }

        [HttpGet("{productId}")]
        public async Task<IActionResult> GetProduct([FromRoute] string productId)
        {
            var response = await _menuService.GetProductDetailsAsync(productId);

            return Ok(new ApiResponse<MenuDetailsResponseDTO>(true, "Product retrieved successfully.", response));


        }
    }
}