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
            //    CreateMap<SignupCustomerRequestDTO, Customer>()
            //    .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));

            //    CreateMap<Customer, SignupCustomerResponseDTO>()
            //                .ForMember(dest => dest.FullName,
            //                           opt => opt.MapFrom(src => $"{src.FirstName} {src.LastName}"));

        }
    }
}
