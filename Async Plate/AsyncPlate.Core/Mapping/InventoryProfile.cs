using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsyncPlate.Core.Entities;
using AsyncPlate.Core.DTOs.Inventory;

namespace AsyncPlate.Core.Mapping
{
    public class InventoryProfile : Profile
    {
        public InventoryProfile()
        {

            CreateMap<AddInventoryRequestDTO, Inventory>();
            CreateMap<Supplier, SupplierSummaryDTO>();
            CreateMap<Inventory, InventoryResponseDTO>();

        }
    }
}
