using AsyncPlate.Core.Common.DTOs;
using AsyncPlate.Core.Common.Extenstions;
using AsyncPlate.Core.DTOs.Authentication;
using AsyncPlate.Core.DTOs.Inventory;
using AsyncPlate.Core.DTOs.Supplier;
using AsyncPlate.Core.Entities;
using AsyncPlate.Core.Interfaces;
using AsyncPlate.Core.Interfaces.Services;
using AsyncPlate.Core.Services.Interfaces;
using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Core.Services.Implementation
{
    public class InventoryService : IInventoryService
    {

        private readonly ILogger<InventoryService> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<AddInventoryRequestDTO> _validator1;
        private readonly IValidator<UpdateInventoryRequestDTO> _validator2;

        public InventoryService(ILogger<InventoryService> logger, IMapper mapper, IUnitOfWork unitOfWork, IValidator<AddInventoryRequestDTO> validator1, IValidator<UpdateInventoryRequestDTO> validator2)
        {
            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _validator1 = validator1;
            _validator2 = validator2;
        }


        public async Task<InventoryResponseDTO> AddInventoryAsync(AddInventoryRequestDTO requestDTO)
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

            var inventory = _mapper.Map<Inventory>(requestDTO);
            inventory.Name = inventory.Name.ToLower();

            var exists = await _unitOfWork.inventories.AnyInventoryAsync(requestDTO.Name.ToLower());

            if (exists)
                throw new Exceptions.BadRequestException("Inventory already exists.");

            //check if the id of the supplier exists in the suppliers table or not
            var supplier = await _unitOfWork.suppliers.GetByIdAsync(requestDTO.SupplierId);
            if (supplier == null)
                throw new Exceptions.BadRequestException("Supplier not found with the provided ID.");

            await _unitOfWork.inventories.AddAsync(inventory);

            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Inventory created successfully: {Name}", inventory.Name);

            //RELOAD THE RELATED RELATIONSHIP : 
            //the mapper will not be able to map the supplier from the supplier_id in the dto so we 
            //need to tell him to get the supplier from the db first => lazy/explicit/eager loading
            inventory = await _unitOfWork.inventories.GetInventoryWithSupplierAsync(inventory.Id);
            var responseDTO = _mapper.Map<InventoryResponseDTO>(inventory);
            return responseDTO;
        }
        public async Task<InventoryResponseDTO> GetInventoryByIdAsync(string inventoryId)
        {
            //check if found/not 
            //return responsedto
            var inventory = await _unitOfWork.inventories.GetInventoryWithSupplierAsync(inventoryId);
            if (inventory == null)
            {
                throw new Exceptions.NotFoundException("Inventory not found.");
            }
            var responseDTO = _mapper.Map<InventoryResponseDTO>(inventory);
            _logger.LogInformation("Inventory retrieved successfully: {Name}", inventory.Name);
            return responseDTO;
        }
        public async Task<PagedResult<InventoryResponseDTO>> GetAllInventoriesAsync(InventoryFilterDTO filterDto)
        {
            var inventoriesQuery = _unitOfWork.inventories.GetAllWithSuppliers();

            if (!string.IsNullOrEmpty(filterDto.Name))
            {
                inventoriesQuery = _unitOfWork.inventories.FilterByName(filterDto.Name);
            }
            var pagedResult = await QueryableExtensions.ToPagedResultAsync(inventoriesQuery, filterDto.PageNumber, filterDto.PageSize);

            var responseDTOs = _mapper.Map<IEnumerable<InventoryResponseDTO>>(pagedResult.Items);
            _logger.LogInformation("Retrieved {Count} inventories (Page {PageNumber} of {TotalPages})", responseDTOs.Count(), filterDto.PageNumber, pagedResult.TotalPages);
            return new PagedResult<InventoryResponseDTO>
            {
                Items = responseDTOs,
                TotalCount = pagedResult.TotalCount,
                PageNumber = filterDto.PageNumber,
                PageSize = filterDto.PageSize,
                TotalPages = pagedResult.TotalPages
            };

        }

        public async Task<InventoryResponseDTO> UpdateInventoryAsync(string inventoryId, UpdateInventoryRequestDTO requestDTO)
        {
            var inventory = await _unitOfWork.inventories.GetByIdAsync(inventoryId);
            if (inventory == null)
            {
                throw new Exceptions.NotFoundException("Inventory not found.");
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
            var updatedInventory = _mapper.Map(requestDTO, inventory);
            //_unitOfWork.inventories.Update(updatedInventory); // No need to call Update if using tracking entities as we use getbyid 
            await _unitOfWork.SaveChangesAsync();

            //reloading
            var inventoryWithSupplier = await _unitOfWork.inventories.GetInventoryWithSupplierAsync(inventoryId);

            _logger.LogInformation("Inventory updated successfully: {Name}", updatedInventory.Name);
            return _mapper.Map<InventoryResponseDTO>(inventoryWithSupplier);

        }
        public async Task<InventoryResponseDTO> DeleteInventoryAsync(string inventoryId)
        {
            var inventory = await _unitOfWork.inventories.GetInventoryWithSupplierAsync(inventoryId);
            if (inventory == null)
            {
                throw new Exceptions.NotFoundException("Inventory not found.");
            }
            _unitOfWork.inventories.Delete(inventory);
            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("Inventory deleted successfully: {Name}", inventory.Name);

            return _mapper.Map<InventoryResponseDTO>(inventory);
        }


        public async Task<PagedResult<InventoryResponseDTO>> GetLowStockInventoriesAsync(InventoryFilterDTO filterDto)
        {
            var inventoriesQuery = _unitOfWork.inventories.GetLowStockInventory();
            if (!string.IsNullOrEmpty(filterDto.Name))
            {
                inventoriesQuery = _unitOfWork.inventories.FilterByName(filterDto.Name);
            }
            var pagedResult = await QueryableExtensions.ToPagedResultAsync(inventoriesQuery, filterDto.PageNumber, filterDto.PageSize);
            
            var responseDTOs = _mapper.Map<IEnumerable<InventoryResponseDTO>>(pagedResult.Items);
            _logger.LogInformation("Retrieved {Count} low stock inventories (Page {PageNumber} of {TotalPages})", responseDTOs.Count(), filterDto.PageNumber, pagedResult.TotalPages);
            return new PagedResult<InventoryResponseDTO>
            {
                Items = responseDTOs,
                TotalCount = pagedResult.TotalCount,
                PageNumber = filterDto.PageNumber,
                PageSize = filterDto.PageSize,
                TotalPages = pagedResult.TotalPages
            };
        }

        public async Task<SupplierSummaryDTO> GetSupplierByInventoryIdAsync(string inventoryId)
        {
            var inventory = await _unitOfWork.inventories.GetInventoryWithSupplierAsync(inventoryId);
            if (inventory == null)
            {
                throw new Exceptions.NotFoundException("Inventory not found.");
            }
            if (inventory.Supplier == null)
            {
                throw new Exceptions.NotFoundException("Supplier not found for the given inventory.");
            }

            var responseDTO = _mapper.Map<SupplierSummaryDTO>(inventory.Supplier);
            _logger.LogInformation("Supplier retrieved successfully for Inventory: {InventoryName}", inventory.Name);
            return responseDTO;
        }
    
    }

}