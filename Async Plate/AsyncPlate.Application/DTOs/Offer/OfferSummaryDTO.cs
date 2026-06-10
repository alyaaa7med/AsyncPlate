using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Application.DTOs.Offer
{
    public class OfferSummaryDTO
    {
        public decimal DiscountPercentage { get; set; }
       
        public bool IsActive { get; set; }
    }
}
