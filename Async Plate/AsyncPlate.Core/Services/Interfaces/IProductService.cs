using AsyncPlate.Core.DTOs.Category;
using AsyncPlate.Core.DTOs.Product;
using AsyncPlate.Core.DTOs.Recipe;
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
        ///Get all
        //Get all by category
        //Update product details
        //Delete product
        //get the best seller 
        //change is available T/F
        //Get all available products
        //Get all unavailable products
        //get all by price range
        //add favorite products for user? will this need a table ?
        Task<IEnumerable<RecipeListDTO>> GetRecipeByProductIdAsync(string productId);

    }
}
