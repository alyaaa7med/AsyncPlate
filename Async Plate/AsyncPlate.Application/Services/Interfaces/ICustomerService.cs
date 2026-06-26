using AsyncPlate.Application.Common.DTOs;
using AsyncPlate.Application.DTOs.Authentication;
using Microsoft.EntityFrameworkCore.Query.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Application.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<PagedResult<SignupCustomerResponseDTO>> GetAllAsync(int pageNumber, int pageSize);


        Task<SignupCustomerResponseDTO> GetByUserIdAsync(string userId);

        Task<PagedResult<SignupCustomerResponseDTO>> GetVipCustomersAsync(int pageNumber, int pageSize);

        Task DeleteByUserIdAsync(string userId);


    }
}
