using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Application.DTOs.Category
{
    public class UpdateCategoryRequestDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public IFormFile? ImageUrl { get; set; }
        public string? OfferId { get; set; }

    }
}
