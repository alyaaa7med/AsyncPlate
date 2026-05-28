using AsyncPlate.Core.DTOs.Inventory;
using AsyncPlate.Core.DTOs.Supplier;
using AsyncPlate.Core.Entities;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Core.Mapping
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
