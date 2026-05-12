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
    public class AuthProfile : Profile
    {
        public AuthProfile()
        {
            CreateMap<AppUser, LoginResponseDTO>()
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.UserType, opt => opt.MapFrom(src => src.UserType.ToString()));

        }
    }
}
