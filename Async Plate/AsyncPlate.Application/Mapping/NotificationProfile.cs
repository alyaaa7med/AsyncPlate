using AsyncPlate.Application.DTOs.Notification;
using AsyncPlate.Domain.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Application.Mapping
{
    public class NotificationProfile : Profile
    {
        public NotificationProfile() {

            CreateMap<CreateNotificationRequestDTO, Notification>();

            CreateMap<Notification, NotificationResponseDTO>()
                .ForMember(dest => dest.UserId,
                   opt => opt.MapFrom(src => src.userId));
                
        }
    }
}
