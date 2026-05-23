using AsyncPlate.Core.Common.DTOs;
using AsyncPlate.Core.DTOs.Supplier;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Core.Services.Interfaces
{
    public interface ISupplierService
    {
        
        Task<SupplierResponseDTO> AddSupplierAsync (AddSupplierRequestDTO requestDTO);
        Task<SupplierResponseDTO> GetSupplierByIdAsync (string supplierId);
        Task<SupplierResponseDTO> UpdateSupplierAsync (string supplierId, UpdateSupplierRequestDTO requestDTO);
        Task<SupplierResponseDTO> DeleteSupplierAsync (string supplierId); 
        Task<PagedResult<SupplierResponseDTO>> GetAllSuppliersAsync(SupplierFilterDTO filterDto);



    }
}
