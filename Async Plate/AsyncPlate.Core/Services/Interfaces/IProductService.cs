using AsyncPlate.Core.DTOs.Category;
using AsyncPlate.Core.DTOs.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Core.Services.Interfaces
{
    public interface IProductService
    {
        Task<ProductResponseDTO> AddProductAsync(AddProductRequestDTO productRequestDTO);
        Task<ProductResponseDTO> GetProductByIdAsync(string productId);
    }
}
