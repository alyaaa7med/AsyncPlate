using AsyncPlate.Application.DTOs.Category;
using AsyncPlate.Application.DTOs.Offer;
using AsyncPlate.Domain.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Application.Mapping
{
    public class OfferProfile : Profile
    {
        public OfferProfile()
        {
            CreateMap<AddOfferRequestDTO, Offer>();


            CreateMap<Offer, OfferResponseDTO>();

            CreateMap<Category, CategorySummaryDTO>();

            CreateMap<Offer, OfferSummaryDTO>();
        }
    }
}
