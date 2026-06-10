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
    public class SupplierProfile : Profile
    {
        public SupplierProfile()
        {
            CreateMap<AddSupplierRequestDTO, Supplier>();
            CreateMap<UpdateSupplierRequestDTO, Supplier>();

            CreateMap<Inventory, InventorySummaryDTO>();
            CreateMap<Supplier, SupplierResponseDTO>();

        }
    }
}
