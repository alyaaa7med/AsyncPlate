using AsyncPlate.Application.Common.DTOs;
using AsyncPlate.Application.DTOs.Category;
using AsyncPlate.Application.DTOs.Product;
using AsyncPlate.Application.DTOs.Recipe;
using AsyncPlate.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Application.Services.Interfaces
{
    public interface IProductService
    {
        Task<ProductResponseDTO> AddProductAsync(AddProductRequestDTO productRequestDTO);
        Task<ProductResponseDTO> GetProductByIdAsync(string productId);
        Task<IEnumerable<RecipeListDTO>> GetRecipeByProductIdAsync(string productId);
        Task MakeProductUnAvailableAsync(string productId);
        Task DeleteProductAsync(string productId);
        Task<PagedResult<ProductResponseDTO>> GetAllProductsAsync(int pageNumber, int pageSize);
        Task<IEnumerable<ProductResponseDTO>> GetProductsByCategoryAsync(string categoryId);
        Task<IEnumerable<ProductResponseDTO>> GetBestSellerProductsAsync();
        Task<IEnumerable<ProductResponseDTO>> GetAvailableProductsAsync();
        Task<IEnumerable<ProductResponseDTO>> GetUnAvailableProductsAsync();
        Task<IEnumerable<ProductResponseDTO>> GetProductsByPriceRangeAsync(decimal minPrice, decimal maxPrice);


        //add favorite products for user? will this need a table ?
        //product status : realtime?

    }
}
