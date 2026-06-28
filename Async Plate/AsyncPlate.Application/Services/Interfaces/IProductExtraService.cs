using AsyncPlate.Application.DTOs.ProductExtra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Application.Services.Interfaces
{
    public interface IProductExtraService
    {
        Task<ProductWithExtrasDTO> AddExtrasAsync(string productId, AddProductExtraDTO dto);

        Task<ProductWithExtrasDTO> UpdateExtrasAsync(string productId, UpdateProductExtrasDTO dto);

        Task<ProductWithExtrasDTO> DeleteExtraAsync(string productId, string extraProductId);
        Task DeleteAllExtrasAsync(string productId);

        Task<IEnumerable<ProductExtraDTO>> GetExtrasByProductIdAsync(string productId);
    }
}
