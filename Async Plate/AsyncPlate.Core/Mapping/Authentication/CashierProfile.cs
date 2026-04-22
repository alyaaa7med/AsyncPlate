using AsyncPlate.Core.DTOs.Authentication;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Core.Mapping.Authentication
{
    public class CashierProfile : Profile
    {
        public CashierProfile() { 

            CreateMap<SignupCashierRequestDTO, Entities.Cashier>();

            CreateMap<Entities.Cashier, SignupCashierResponseDTO>();
        }

    }
}
