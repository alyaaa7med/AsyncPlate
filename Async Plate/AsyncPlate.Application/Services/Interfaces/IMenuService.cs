using AsyncPlate.Application.Common.DTOs;
using AsyncPlate.Application.DTOs.Menu;
using AsyncPlate.Application.Services.Implementation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Application.Services.Interfaces
{
    public interface IMenuService
    {
        Task<PagedResult<MenuItemResponseDTO>> GetMenuAsync(MenuFilterDTO filter);

        Task<MenuDetailsResponseDTO> GetProductDetailsAsync(string productId);
    }
}
