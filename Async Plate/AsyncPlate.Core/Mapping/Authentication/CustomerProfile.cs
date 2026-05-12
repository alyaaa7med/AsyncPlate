using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AsyncPlate.Core.Entities;
using AsyncPlate.Core.DTOs.Authentication;

namespace AsyncPlate.Core.Mapping.Authentication
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {


            CreateMap<SignupAppUserRequestDTO, AppUser>()
                .ForMember(dest => dest.UserName,
                    opt => opt.MapFrom(src => src.Email)) // غالبًا اليوزرنيم = الإيميل
                .ForMember(dest => dest.ProfilePictureUrl,
                    opt => opt.Ignore()); // لأن IFormFile يحتاج معالجة منفصلة

            CreateMap<AppUser, SignupAppUserResponseDTO>()
                .ForMember(dest => dest.FullName,
                    opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
                .ForMember(dest => dest.ProfilePicture,
                    opt => opt.MapFrom(src => string.IsNullOrEmpty(src.ProfilePictureUrl)
                        ? null
                        : $"https://localhost:51499{src.ProfilePictureUrl.Replace("\\", "/")}"));

            // Customer Request DTO -> Customer Entity
            CreateMap<SignupCustomerRequestDTO, Customer>();


            // Customer Entity -> Customer Response DTO
            CreateMap<Customer, SignupCustomerResponseDTO>();

        }
    }
}
