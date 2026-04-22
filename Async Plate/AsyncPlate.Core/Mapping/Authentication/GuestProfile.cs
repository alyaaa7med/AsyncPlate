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
    public class GuestProfile : Profile
    {
        public GuestProfile()
        {
            CreateMap<SignupGuestRequestDTO, Guest>()
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));

            CreateMap<Guest, SignupGuestResponseDTO>()
                        .ForMember(dest => dest.FullName,
                                   opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));

        }
    }
}
