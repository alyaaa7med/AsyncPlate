using AsyncPlate.Application.DTOs.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Application.Services.Interfaces
{
    public interface ICategoryService
    {
        Task<CategoryResponseDTO> AddCategoryAsync(AddCategoryRequestDTO categoryRequestDTO);
        Task<CategoryResponseDTO> GetCategoryByIdAsync(string categoryId);
        //Task<CategoryResponseDTO> UpdateCategoryAsync(int categoryId, UpdateCategoryRequestDTO categoryRequestDTO);

        //delete 
        //get all
        //get offers per category


    }
}
