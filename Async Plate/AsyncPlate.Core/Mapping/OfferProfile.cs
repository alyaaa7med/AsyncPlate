using AsyncPlate.Core.DTOs.Offer;
using AsyncPlate.Core.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Core.Mapping
{
    public class OfferProfile : Profile
    {
        public OfferProfile()
        {
            //CreateMap<AddOfferRequestDTO, Offer>();
            //CreateMap<Offer, OfferResponseDTO>();
            CreateMap<Offer, OfferSummaryDTO>();
        }
    }
}
