using AsyncPlate.Application.Common.DTOs;
using AsyncPlate.Application.DTOs.Category;
using AsyncPlate.Application.Interfaces;
using AsyncPlate.Application.Interfaces.Services;
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
        Task<PagedResult<CategoryResponseDTO>> GetAllAsync(int pageNumber, int pageSize);
        Task DeleteCategoryAsync(string categoryId);
        Task<CategoryResponseDTO> UpdateCategoryAsync(string categoryId, UpdateCategoryRequestDTO categoryRequestDTO);


    }
}
