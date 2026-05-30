using AsyncPlate.Core.DTOs.Category;
using AsyncPlate.Core.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Core.Mapping
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<AddCategoryRequestDTO, Category>()
                .ForMember(dest => dest.ImageUrl,
                   opt => opt.Ignore());

            CreateMap<Category, CategoryResponseDTO>()
                 .ForMember(dest => dest.ImageUrl,
                 //it will not be null 
                    opt => opt.MapFrom(src => string.IsNullOrEmpty(src.ImageUrl)
                        ? null
                        : $"https://localhost:51499{src.ImageUrl.Replace("\\", "/")}"))

                 .ForMember(dest => dest.Offer, opt => opt.MapFrom(src => src.CurrentOffer));

            CreateMap<Category, CategorySummaryDTO>();
        }
    }
}
