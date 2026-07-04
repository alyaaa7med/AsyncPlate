using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Application.DTOs.Review
{
    public class ReviewResponseDTO
    {

        public string Id { get; set; } = null!;

        public int Rating { get; set; }

        public string Message { get; set; } = string.Empty;
        public string OrderId { get; set; } = string.Empty;
    }
}
