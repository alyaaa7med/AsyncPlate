using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Application.DTOs.Review
{
    public class UpdateReviewRequestDTO
    {
        public int Rating { get; set; }

        public string Message { get; set; } = string.Empty;
    }
}
