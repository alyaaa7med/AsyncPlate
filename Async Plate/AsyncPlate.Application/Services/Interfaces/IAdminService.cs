using AsyncPlate.Application.DTOs.Admin;
using AsyncPlate.Application.DTOs.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Application.Services.Interfaces
{
    public interface IAdminService
    {
        Task<AdminResponseDTO> CreateAdminAsync(CreateAdminRequestDTO requestDTO);

    }
}
