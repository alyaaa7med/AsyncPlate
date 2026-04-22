using AsyncPlate.Core.DTOs.Authentication;
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
        public KitchenChefProfile() {

            //match the same (incase sensitive)
            //ignore the other 
            //override using map for if you need other mapping

            CreateMap<SignupKitchenChefRequestDTO, Entities.KitchenChef>();

            CreateMap<Entities.KitchenChef, SignupKitchenChefResponseDTO>();
        }
    }
}
