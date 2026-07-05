using AsyncPlate.API.Models;
using AsyncPlate.Application.Common.DTOs;
using AsyncPlate.Application.DTOs.Notification;
using AsyncPlate.Application.DTOs.Supplier;
using AsyncPlate.Application.Services.Implementation;
using AsyncPlate.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace AsyncPlate.API.Controllers
{
    [ApiController]
    [Route("api/[Controller]s")]
    [Authorize] //to take the id from jwt 
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;

        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllNotifications(int pageNumber = 1, int pageSize = 10)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var responseDTOs = await _notificationService.GetAllNotificationsAsync(userId, pageNumber, pageSize);
            return Ok(new ApiResponse<PagedResult<NotificationResponseDTO>>
                (true, "Notifications retrieved successfully", responseDTOs));
        }

        [HttpGet("{notificationId}")]

        public async Task<IActionResult> GetNotification([FromRoute] string notificationId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var responseDTO = await _notificationService.GetNotificationByIdAsync(userId, notificationId);
            return Ok(new ApiResponse<NotificationResponseDTO>(true, "Notification retrieved successfully", responseDTO));
        }

        [HttpPatch("{notificationId}/read")]
        public async Task<IActionResult> MarkNotificationAsRead([FromRoute] string notificationId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (string.IsNullOrEmpty(userId))
                return Unauthorized();

            var responseDTO = await _notificationService.MarkNotificationAsReadAsync(userId, notificationId);
            return Ok(new ApiResponse<NotificationResponseDTO>(true, "Notification was read successfully", responseDTO));
        }
    }

    
}
