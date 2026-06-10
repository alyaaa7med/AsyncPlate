using AsyncPlate.Application.DTOs.Inventory;
using AsyncPlate.Application.DTOs.Supplier;
using AsyncPlate.Domain.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Application.Mapping
{
    public class InventoryProfile : Profile
    {
        public InventoryProfile()
        {

            CreateMap<AddInventoryRequestDTO, Inventory>();
            CreateMap<UpdateInventoryRequestDTO, Inventory>();

            CreateMap<Supplier, SupplierSummaryDTO>();
            CreateMap<Inventory, InventoryResponseDTO>();

        }
    }
}
