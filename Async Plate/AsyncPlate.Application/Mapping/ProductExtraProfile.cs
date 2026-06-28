using AsyncPlate.Application.DTOs.Authentication;
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
    public class ProductExtraProfile : Profile
    {
        public ProductExtraProfile()
        {
            
            CreateMap< Product, ProductWithExtrasDTO>();

        }
    }
}
