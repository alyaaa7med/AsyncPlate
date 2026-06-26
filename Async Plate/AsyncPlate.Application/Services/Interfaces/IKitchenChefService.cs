using AsyncPlate.Application.Common.DTOs;
using AsyncPlate.Application.DTOs.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Application.Services.Interfaces
{
    public interface  IKitchenChefService
    {
       
        Task<PagedResult<SignupKitchenChefResponseDTO>> GetAllAsync(int pageNumber, int pageSize);

        Task<SignupKitchenChefResponseDTO> GetByUserIdAsync(string userId);

        Task DeleteByUserIdAsync(string userId);


    }
}
