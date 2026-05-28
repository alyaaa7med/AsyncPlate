using AsyncPlate.Core.DTOs.Admin;
using AsyncPlate.Core.DTOs.Authentication;
using AsyncPlate.Core.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Core.Mapping
{
    public class AdminProfile : Profile
    {
        public AdminProfile()
        {


            CreateMap<CreateAdminRequestDTO, Admin>();


            CreateMap<Admin, AdminResponseDTO>();

        }
    }
}
