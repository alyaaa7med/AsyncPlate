using AsyncPlate.Application.Common.DTOs;
using AsyncPlate.Application.Common.Extenstions;
using AsyncPlate.Application.DTOs.Inventory;
using AsyncPlate.Application.DTOs.Supplier;
using AsyncPlate.Application.Interfaces;
using AsyncPlate.Application.Interfaces.Services;
using AsyncPlate.Application.Services.Interfaces;
using AsyncPlate.Domain.Entities;
using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Application.Services.Implementation
{
    public class SupplierService : ISupplierService
    {
        private readonly ILogger<SupplierService> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<AddSupplierRequestDTO> _validator1;
        private readonly IValidator<UpdateSupplierRequestDTO> _validator2;
        private readonly IEmailService _emailService;


        public SupplierService(ILogger<SupplierService> logger, IMapper mapper, IUnitOfWork unitOfWork, IValidator<AddSupplierRequestDTO> validator1, IValidator<UpdateSupplierRequestDTO> validator2, IEmailService emailService)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _validator1 = validator1;
            _validator2 = validator2;
            _emailService = emailService;
        }

        public async Task<SupplierResponseDTO> AddSupplierAsync(AddSupplierRequestDTO requestDTO)
        {
            //validate dto
            //mapping 
            //add to db
            //mappig + return dto


            var validationResult = await _validator1.ValidateAsync(requestDTO);

            if (!validationResult.IsValid)
            {
                var errorsDictionary = validationResult.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray()
                    );

                throw new Exceptions.ValidationException(errorsDictionary);
            }

            var supplier = _mapper.Map<Supplier>(requestDTO);

            var exists = await _unitOfWork.suppliers.AnySupplierAsync(requestDTO.ContactEmail);

            if (exists)
                throw new Exceptions.BadRequestException("Supplier already exists.");

            await _unitOfWork.suppliers.AddAsync(supplier);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Supplier created successfully: {Email}", supplier.ContactEmail);

            var responseDTO = _mapper.Map<SupplierResponseDTO>(supplier);
            return responseDTO;
        }
        public async Task<SupplierResponseDTO> GetSupplierByIdAsync(string supplierId)
        {
            //check if found/not 
            //return responsedto
            var supplier = await _unitOfWork.suppliers.GetByIdAsync(supplierId);
            if (supplier == null)
            {
                throw new Exceptions.NotFoundException("Supplier not found.");
            }
            var responseDTO = _mapper.Map<SupplierResponseDTO>(supplier);
            _logger.LogInformation("Supplier retrieved successfully: {Email}", supplier.ContactEmail);
            return responseDTO;
        }
        public async Task<PagedResult<SupplierResponseDTO>> GetAllSuppliersAsync(SupplierFilterDTO filterDto)
        {
            var suppliersQuery = _unitOfWork.suppliers.GetAll();

            if (!string.IsNullOrEmpty(filterDto.Name))
            {
                suppliersQuery = _unitOfWork.suppliers.FilterByName(filterDto.Name);
            }
            var pagedResult = await suppliersQuery.ToPagedResultAsync(filterDto.PageNumber, filterDto.PageSize);

            var responseDTOs = _mapper.Map<IEnumerable<SupplierResponseDTO>>(pagedResult.Items);

            _logger.LogInformation("Retrieved {Count} suppliers (Page {PageNumber} of {TotalPages})", responseDTOs.Count(), filterDto.PageNumber, pagedResult.TotalPages);
            return new PagedResult<SupplierResponseDTO>
            {
                Items = responseDTOs,
                TotalCount = pagedResult.TotalCount,
                PageNumber = filterDto.PageNumber,
                PageSize = filterDto.PageSize,
                TotalPages = pagedResult.TotalPages
            };

        }

        public async Task<SupplierResponseDTO> UpdateSupplierAsync(string supplierId, UpdateSupplierRequestDTO requestDTO)
        {
            var supplier = await _unitOfWork.suppliers.GetByIdAsync(supplierId);
            if (supplier == null)
            {
                throw new Exceptions.NotFoundException("Supplier not found.");
            }

            var validationResult = await _validator2.ValidateAsync(requestDTO);

            if (!validationResult.IsValid)
            {
                var errorsDictionary = validationResult.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray()
                    );

                throw new Exceptions.ValidationException(errorsDictionary);
            }
            var updatedSupplier = _mapper.Map(requestDTO, supplier);
            //_unitOfWork.suppliers.Update(updatedSupplier); // No need to call Update if using tracking entities as we use getbyid 
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Supplier updated successfully: {Email}", updatedSupplier.ContactEmail);
            return _mapper.Map<SupplierResponseDTO>(updatedSupplier);


        }
        public async Task<SupplierResponseDTO> DeleteSupplierAsync(string supplierId)
        {
            var supplier = await _unitOfWork.suppliers.GetByIdAsync(supplierId);
            if (supplier == null)
            {
                throw new Exceptions.NotFoundException("Supplier not found.");
            }
            _unitOfWork.suppliers.Delete(supplier);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("Supplier deleted successfully: {Email}", supplier.ContactEmail);

            return _mapper.Map<SupplierResponseDTO>(supplier);
        }


        public async Task<PagedResult<InventorySummaryDTO>> GetAllInventoriesBySupplierIdAsync(string supplierId, InventoryFilterDTO filterDTO)
        {
            var supplier = await _unitOfWork.suppliers.GetByIdAsync(supplierId);
            if (supplier == null)
            {
                throw new Exceptions.NotFoundException("Supplier not found.");
            }

            var inventoriesQuery = _unitOfWork.inventories.GetInventoriesBySupplierId(supplierId);

            var pagedResult = await inventoriesQuery.ToPagedResultAsync(filterDTO.PageNumber, filterDTO.PageSize); 
            
            var responseDTOs = _mapper.Map<IEnumerable<InventorySummaryDTO>>(pagedResult.Items);

            _logger.LogInformation("Retrieved {Count} inventories for supplier {Email}", responseDTOs.Count(), supplier.ContactEmail);
            return new PagedResult<InventorySummaryDTO>
            {
                Items = responseDTOs,
                TotalCount = pagedResult.TotalCount,
                PageNumber = filterDTO.PageNumber,
                PageSize = filterDTO.PageSize,
                TotalPages = pagedResult.TotalPages
            };
        }
    }
}