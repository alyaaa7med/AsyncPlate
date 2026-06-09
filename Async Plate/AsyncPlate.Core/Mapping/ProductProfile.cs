using AsyncPlate.Application.DTOs.Product;
using AsyncPlate.Domain.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Application.Mapping
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<AddProductRequestDTO, Product>()
                .ForMember(d => d.Type, o => o.MapFrom(s => Enum.Parse<Domain.Entities.Type>
                (s.Type, true)));

            CreateMap<Product, ProductResponseDTO>()
                .ForMember(d => d.Type,
                    o => o.MapFrom(s => s.Type.ToString()))
                .ForMember(d => d.CategorySummary, o => o.MapFrom(s => s.Category));

            CreateMap<Product, ProductSummaryDTO>();


        }
    }
}
