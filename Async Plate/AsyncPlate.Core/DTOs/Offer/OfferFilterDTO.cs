using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Application.DTOs.Offer
{
    public class OfferFilterDTO
    {
        //we can filter by category name, percentage, and active status


        //filtering 
        public string? CategoryName { get; set; }
        public decimal? Percentage { get; set; }
        public bool? IsActive { get; set; }

        //pagination
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
