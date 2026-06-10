using AsyncPlate.Application.DTOs.Admin;
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
    public class AdminProfile : Profile
    {
        public AdminProfile()
        {


            CreateMap<CreateAdminRequestDTO, Admin>();


            CreateMap<Admin, AdminResponseDTO>();

        }
    }
}
