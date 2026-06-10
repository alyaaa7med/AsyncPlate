using AsyncPlate.Application.DTOs.Order;
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
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<MakeOrderRequestDTO, Order>();

            CreateMap<OrderItemRequestDTO, OrderItem>()
                .ForMember(o => o.Extras, o => o.MapFrom(s => s.ExtraItems));

            CreateMap<OrderExtraItemRequestDTO, OrderExtraItem>();





            CreateMap<Order, OrderResponseDTO>()
                  .ForMember(o => o.Status,o => o.MapFrom(s => s.Status.ToString()));

            CreateMap<OrderItem, OrderItemResponseDTO>();

            CreateMap<OrderExtraItem, OrderExtraItemResponseDTO>();
        }
    }
}
