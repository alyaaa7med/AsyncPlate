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
    public class KitchenChefProfile : Profile
    {
        public KitchenChefProfile()
        {

            //match the same (incase sensitive)
            //ignore the other 
            //if field = null it is mapped as null not runtime error
            //override using map for if you need other mapping

         


            CreateMap<SignupKitchenChefRequestDTO, KitchenChef>();

            CreateMap<KitchenChef, SignupKitchenChefResponseDTO>();


          
           

        }

    }
}