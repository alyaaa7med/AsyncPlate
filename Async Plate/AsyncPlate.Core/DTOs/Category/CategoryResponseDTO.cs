using AsyncPlate.Application.DTOs.Offer;
using AsyncPlate.Application.DTOs.Product;
using AsyncPlate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Application.DTOs.Category
{
    public class CategoryResponseDTO
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public OfferSummaryDTO Offer { get; set; } = new ();

        public IEnumerable<ProductSummaryDTO> Products { get; set; } = new List<ProductSummaryDTO>();

    }
}
