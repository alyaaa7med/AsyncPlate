using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Application.DTOs.Offer
{
    public class AddOfferRequestDTO
    {
        public string Name { get; set; } = null!;
        public string Title { get; set; } = null!;
        public decimal DiscountPercentage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<string> CategoryIds { get; set; } = new List<string>();
    }
}
