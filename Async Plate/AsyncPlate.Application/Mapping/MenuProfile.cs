using AsyncPlate.Application.DTOs.Menu;
using AsyncPlate.Application.DTOs.Offer;
using AsyncPlate.Application.DTOs.Product;
using AsyncPlate.Application.DTOs.ProductExtra;
using AsyncPlate.Domain.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Application.Mapping
{
    public class MenuProfile : Profile
    {
        public MenuProfile()
        {
            CreateMap<Product, MenuItemResponseDTO>()
                    .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                    .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()));

            CreateMap<Product, MenuDetailsResponseDTO>()
                    .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                    .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.Type.ToString()))       
                    .ForMember(dest => dest.Extras, opt => opt.MapFrom(src => src.MainProducts.Select(mp => mp.ExtraProduct)));

            CreateMap<Product, ProductExtraDTO>();
        }
    }
}

