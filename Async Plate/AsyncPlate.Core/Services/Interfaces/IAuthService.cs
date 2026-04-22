
using AsyncPlate.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsyncPlate.Core.DTOs.Authentication;

namespace AsyncPlate.Core.Services.Interfaces
{
    public interface IAuthService
    {
        Task<SignupGuestResponseDTO> SignUpGuestAsync(SignupGuestRequestDTO requestDTO);
        Task<SignupKitchenChefResponseDTO> SignUpKitchenChefAsync(SignupKitchenChefRequestDTO requestDTO);
        Task<SignupCashierResponseDTO> SignUpCashierAsync(SignupCashierRequestDTO requestDTO);
        Task SignInAsync();
        Task SendEmailAsync();
    }
}
