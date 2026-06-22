using AsyncPlate.Application.DTOs.Authentication;
using AsyncPlate.Domain.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Application.Mapping
{
    public class AuthProfile : Profile
    {
        public AuthProfile()
        {
            CreateMap<SignupAppUserRequestDTO, AppUser>() //will be used by all user types (Customer, KitchenChef, Admin)
               .ForMember(dest => dest.UserName,
                   opt => opt.MapFrom(src => src.Email)) //  اليوزرنيم = الإيميل
               .ForMember(dest => dest.ProfilePictureUrl,
                   opt => opt.Ignore()); // لأن IFormFile يحتاج معالجة منفصلة

            CreateMap<AppUser, SignupAppUserResponseDTO>()
                .ForMember(dest => dest.FullName,
                    opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"))
                .ForMember(dest => dest.ProfilePicture,
                    opt => opt.MapFrom(src => string.IsNullOrEmpty(src.ProfilePictureUrl)
                        ? null
                        : $"https://localhost:51499{src.ProfilePictureUrl.Replace("\\", "/")}"));

            CreateMap<AppUser, LoginResponseDTO>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserType, opt => opt.MapFrom(src => src.UserType.ToString()));

        }
    }
}
