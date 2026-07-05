using AsyncPlate.Application.DTOs.ProductExtra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Application.DTOs.Menu
{
    public class MenuDetailsResponseDTO
    {
        public string Id { get; set; }= string.Empty;

        public string Name { get; set; }= string.Empty;


        public string Type { get; set; }= string.Empty;

        public string CategoryName { get; set; }= string.Empty;

        public decimal BasePrice { get; set; }

        public decimal FinalPrice { get; set; }

        public decimal? DiscountPercentage { get; set; }

        public bool HasOffer { get; set; }
        public bool IsAvailable { get; set; }


        public List<ProductExtraDTO> Extras { get; set; } = [];
    }
}
