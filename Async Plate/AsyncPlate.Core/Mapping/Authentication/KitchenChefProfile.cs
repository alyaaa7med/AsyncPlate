using AsyncPlate.Core.DTOs.Authentication;
using AsyncPlate.Core.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Core.Mapping.Authentication
{
    public class KitchenChefProfile : Profile
    {
        public KitchenChefProfile()
        {

            //match the same (incase sensitive)
            //ignore the other 
            //if field = null it is mapped as null not runtime error
            //override using map for if you need other mapping

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


            CreateMap<SignupKitchenChefRequestDTO, KitchenChef>();

            CreateMap<KitchenChef, SignupKitchenChefResponseDTO>();


          
           

        }

    }
}