using AsyncPlate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Application.DTOs.Notification
{
    public class NotificationResponseDTO
    {
        public string Id { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsRead { get; set; } 
        public string Url { get; set; } = string.Empty;

        public string UserId { get; set; } = string.Empty;

    }
}
