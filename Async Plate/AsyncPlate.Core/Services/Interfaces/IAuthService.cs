
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
        Task<SignupCustomerResponseDTO> SignUpCustomerAsync(SignupCustomerRequestDTO requestDTO);
        Task<SignupKitchenChefResponseDTO> SignUpKitchenChefAsync(SignupKitchenChefRequestDTO requestDTO);
        Task<LoginResponseDTO> LoginAsync(LoginRequestDTO requestDTO);
        Task ForgetPasswordAsync(ForgetPasswordRequestDTO requestDTO);
        Task ResetPasswordAsync(ResetPasswordRequestDTO requestDTO);
        Task LogoutAsync(string userId);
        Task<RefreshTokenResponseDTO> RefreshTokenAsync(RefreshTokenRequestDTO requestDTO);

        //Task SendEmailAsync();
    }
}
