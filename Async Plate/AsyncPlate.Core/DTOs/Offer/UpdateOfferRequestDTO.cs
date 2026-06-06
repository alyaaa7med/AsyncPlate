using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Core.DTOs.Offer
{
    public class UpdateOfferRequestDTO
    {
        public string Title { get; set; } = string.Empty;
        public decimal DiscountPercentage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool IsActive { get; set; }

        public List<string> CategoryIds { get; set; } = new List<string>();
    }
}
