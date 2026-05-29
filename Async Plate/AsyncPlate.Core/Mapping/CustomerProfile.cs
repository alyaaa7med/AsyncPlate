using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AsyncPlate.Core.Entities;
using AsyncPlate.Core.DTOs.Authentication;

namespace AsyncPlate.Core.Mapping
{
    public class CustomerProfile : Profile
    {
        public CustomerProfile()
        {


           

            // Customer Request DTO -> Customer Entity
            CreateMap<SignupCustomerRequestDTO, Customer>();


            // Customer Entity -> Customer Response DTO
            CreateMap<Customer, SignupCustomerResponseDTO>();

        }
    }
}
