using AsyncPlate.Core.DTOs.Admin;
using AsyncPlate.Core.DTOs.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Core.Services.Interfaces
{
    public interface IAdminService
    {
        Task<AdminResponseDTO> CreateAdminAsync(CreateAdminRequestDTO requestDTO);

    }
}
