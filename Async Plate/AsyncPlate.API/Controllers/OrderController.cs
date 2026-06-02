using AsyncPlate.API.Models;
using AsyncPlate.Core.DTOs.Order;
using AsyncPlate.Core.DTOs.Product;
using AsyncPlate.Core.Services.Implementation;
using AsyncPlate.Core.Services.Interfaces;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace AsyncPlate.API.Controllers
{
    [ApiController]
    [Route("api/[controller]s")]
    //[Authorize(Roles = "Customers")]

    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> MakeOrder([FromBody] MakeOrderRequestDTO makeOrderRequestDTO)
        {
            var responseDto = await _orderService.MakeOrderAsync(makeOrderRequestDTO);
            return Created($"/orders/{responseDto.Id}", new ApiResponse<OrderResponseDTO>(true, "Order created successfully", responseDto));
        }

        [HttpPost("{orderId}/cook")]
        public async Task<IActionResult> CookOrder([FromRoute] string orderId)
        {
            await _orderService.CookOrderAsync(orderId);
            return Ok(new ApiResponse<OrderResponseDTO>(true, "Order cooked successfully", null));
        }

        [HttpPost("{orderId}/confirm")]
        public async Task<IActionResult> ConfirmOrder([FromRoute] string orderId)
        {
            var responseDto = await _orderService.ConfirmOrderAsync(orderId);
            return Ok(new ApiResponse<OrderResponseDTO>(true, "Order confirmed successfully", responseDto));
        }


        [HttpPost("{orderId}/cancel")]
        public async Task<IActionResult> CancelOrder([FromRoute] string orderId)
        {
            var responseDto = await _orderService.CancelOrderAsync(orderId);
            return Ok(new ApiResponse<OrderResponseDTO>(true, "Order canceled successfully", responseDto));
        }


        [HttpPost("{orderId}/complete")]
        public async Task<IActionResult> CompleteOrder([FromRoute] string orderId)
        {
            var responseDto = await _orderService.CompleteOrderAsync(orderId);
            return Ok(new ApiResponse<OrderResponseDTO>(true, "Order completed successfully", responseDto));
        }


    }
}

 