using AsyncPlate.API.Models;
using AsyncPlate.Application.DTOs.Order;
using AsyncPlate.Application.Services.Interfaces;
using AsyncPlate.Application.DTOs.Product;
using AsyncPlate.Application.Services.Implementation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using AsyncPlate.Application.Common.DTOs;

namespace AsyncPlate.API.Controllers
{
    [ApiController]
    [Route("api/[controller]s")]

    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> MakeOrder([FromBody] MakeOrderRequestDTO makeOrderRequestDTO)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var responseDto = await _orderService.MakeOrderAsync(userId, makeOrderRequestDTO);
            return Created($"/orders/{responseDto.Id}", new ApiResponse<OrderResponseDTO>(true, "Order created successfully", responseDto));
        }


        [HttpPatch("{orderId}/confirm")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> ConfirmOrder([FromRoute] string orderId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();
            var responseDto = await _orderService.ConfirmOrderAsync(orderId, userId);
            return Ok(new ApiResponse<OrderResponseDTO>(true, "Order confirmed successfully", responseDto));
        }


        [HttpPatch("{orderId}/cancel")]
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> CancelOrder([FromRoute] string orderId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();
            var responseDto = await _orderService.CancelOrderAsync(orderId, userId);
            return Ok(new ApiResponse<OrderResponseDTO>(true, "Order cancelled successfully", responseDto));


        }


        [HttpPatch("{orderId}/cook")]
        [Authorize(Roles = "KitchenChef")]
        public async Task<IActionResult> CookOrder([FromRoute] string orderId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            await _orderService.CookOrderAsync(orderId, userId);
            return Ok(new ApiResponse<OrderResponseDTO>(true, "Order cooked successfully", null));
        }

        [HttpPatch("{orderId}/complete")]
        [Authorize(Roles = "KitchenChef")]
        public async Task<IActionResult> CompleteOrder([FromRoute] string orderId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var responseDto = await _orderService.CompleteOrderAsync(orderId, userId);
            return Ok(new ApiResponse<OrderResponseDTO>(true, "Order ready to be delivered", responseDto));

        }


        [HttpGet("my-active-orders")]
        [Authorize(Roles = "KitchenChef")]
        public async Task<IActionResult> GetChefActiveOrders()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var responseDto = await _orderService.GetChefActiveOrdersAsync(userId);
            return Ok(new ApiResponse<IEnumerable<OrderResponseDTO>>(true, "Active orders retrieved successfully", responseDto));

        }

        [HttpGet("{orderId}")]
        [Authorize(Roles = "Admin,KitchenChef")]
        public async Task<IActionResult> GetOrderById([FromRoute] string orderId)
        {
            var responseDto = await _orderService.GetOrderByIdAsync(orderId);
            return Ok(new ApiResponse<OrderResponseDTO>(true, "Order retrieved successfully", responseDto));

        }

        [HttpGet]
        [Authorize(Roles = "Admin,KitchenChef")]
        public async Task<IActionResult> GetAllOrdersWithRelatedData([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10)
        {
            var pagedResult = await _orderService.GetAllOrdersWithRelatedDataAsync(pageNumber, pageSize);
            return Ok(new ApiResponse<PagedResult<OrderResponseDTO>>(true, "Orders retrieved successfully", pagedResult));
        }
    }
}

