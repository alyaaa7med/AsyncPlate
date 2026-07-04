using AsyncPlate.Application.DTOs.Review;
using AsyncPlate.Domain.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Application.Mapping
{
    public class ReviewProfile : Profile
    {
        public ReviewProfile()
        {
            CreateMap<AddReviewRequestDTO, Review>();

            CreateMap<UpdateReviewRequestDTO, Review>();
            CreateMap<Review, ReviewResponseDTO>();


        }
    }
}
