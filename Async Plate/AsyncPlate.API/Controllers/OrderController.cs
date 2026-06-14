using AsyncPlate.API.Models;
using AsyncPlate.Application.DTOs.Order;
using AsyncPlate.Application.Services.Interfaces;
using AsyncPlate.Application.DTOs.Product;
using AsyncPlate.Application.Services.Implementation;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

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
        //[Authorize(Roles = "Customer")]
        public async Task<IActionResult> MakeOrder([FromBody] MakeOrderRequestDTO makeOrderRequestDTO)
        {
            
            var responseDto = await _orderService.MakeOrderAsync(makeOrderRequestDTO);
            return Created($"/orders/{responseDto.Id}", new ApiResponse<OrderResponseDTO>(true, "Order created successfully", responseDto));
        }

        [HttpPost("{orderId}/cook")]
        //[Authorize(Roles = "KitchenChef")]
        public async Task<IActionResult> CookOrder([FromRoute] string orderId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();
            await _orderService.CookOrderAsync(orderId, userId);
            return Ok(new ApiResponse<OrderResponseDTO>(true, "Order cooked successfully", null));
        }

        [HttpPost("{orderId}/confirm")]
        //[Authorize(Roles = "Customer")]
        public async Task<IActionResult> ConfirmOrder([FromRoute] string orderId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
                return Unauthorized();
            var responseDto = await _orderService.ConfirmOrderAsync(orderId, userId);
            return Ok(new ApiResponse<OrderResponseDTO>(true, "Order confirmed successfully", responseDto));
        }


        [HttpPost("{orderId}/cancel")]
        //[Authorize(Roles = "Customer,KitchenChef")]
        public async Task<IActionResult> CancelOrder([FromRoute] string orderId)
        {
            var responseDto = await _orderService.CancelOrderAsync(orderId);
            return Ok(new ApiResponse<OrderResponseDTO>(true, "Order canceled successfully", responseDto));
        }


     


    }
}

/*
 POST /api/orders
GET  /api/orders/my
GET  /api/orders/{id}
PUT  /api/orders/{id}/cancel

GET /api/chef/orders/live
PUT /api/chef/orders/{id}/start-cooking
PUT /api/chef/orders/{id}/ready

GET /api/admin/orders
GET /api/admin/orders/history
GET /api/admin/dashboard/stats
GET /api/admin/reports/revenue
 */