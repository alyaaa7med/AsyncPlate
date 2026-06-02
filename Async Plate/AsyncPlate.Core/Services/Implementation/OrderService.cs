using AsyncPlate.Core.DTOs.Inventory;
using AsyncPlate.Core.DTOs.Order;
using AsyncPlate.Core.Entities;
using AsyncPlate.Core.Exceptions;
using AsyncPlate.Core.Interfaces;
using AsyncPlate.Core.Interfaces.Repositories;
using AsyncPlate.Core.Services.Interfaces;
using AutoMapper;
using FluentValidation;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Core.Services.Implementation
{
    public class OrderService : IOrderService
    {
        private readonly ILogger<IOrderService> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<MakeOrderRequestDTO> _validator1;
        private readonly ICustomerRepo _customerRepo;
        private readonly IProductRepo _productRepo;
        private readonly IRecipeRepo _recipeRepo;
        private readonly IProductExtraRepo _productExtraRepo;
        private readonly IOrderRepo _orderRepo;
        private readonly IOrderItemRepo _orderItemRepo;
        private readonly IOrderExtraItemRepo _orderExtraItemRepo;
        private readonly IInventoryRepo _inventoryRepo;

        public OrderService(ILogger<IOrderService> logger, IMapper mapper, IUnitOfWork unitOfWork,
            IValidator<MakeOrderRequestDTO> validator1, ICustomerRepo customerRepo
            , IProductRepo productRepo, IProductExtraRepo productExtraRepo, IRecipeRepo recipeRepo
            , IOrderRepo orderRepo, IOrderItemRepo orderItemRepo, IOrderExtraItemRepo orderExtraItemRepo, IInventoryRepo inventoryRepo)
        {

            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _validator1 = validator1;
            //_validator2 = validator2; no need for this as faluent validation will automatically resolve the validator for OrderItemRequestDTO when validating the MakeOrderRequestDTO which contains a list of OrderItemRequestDTO
            _customerRepo = customerRepo;
            _productRepo = productRepo;
            _recipeRepo = recipeRepo;
            _productExtraRepo = productExtraRepo;
            _orderRepo = orderRepo;
            _orderItemRepo = orderItemRepo;
            _orderExtraItemRepo = orderExtraItemRepo;
            _inventoryRepo = inventoryRepo;

        }
        public async Task<OrderResponseDTO> MakeOrderAsync(MakeOrderRequestDTO makeOrderRequestDTO)
        {
            //validate dto
            //chef if customer is found  
            //check if product is found
            //check productextra related to product or not
            //mapping 
            //save order 
            //map and return 
            var validationResult = await _validator1.ValidateAsync(makeOrderRequestDTO);

            if (!validationResult.IsValid)
            {
                var errorsDictionary = validationResult.Errors
                    .GroupBy(e => e.PropertyName)
                    .ToDictionary(
                        g => g.Key,
                        g => g.Select(e => e.ErrorMessage).ToArray()
                    );
                _logger.LogWarning("");
                throw new Exceptions.ValidationException(errorsDictionary);
            }

            var customer = await _customerRepo.GetByIdAsync(makeOrderRequestDTO.CustomerId);
            if (customer == null)
            {
                throw new Exceptions.NotFoundException($"Customer with id {makeOrderRequestDTO.CustomerId} not found");
            }
            decimal totalprice = 0;
            foreach (var orderItem in makeOrderRequestDTO.OrderItems)
            {
                var product = await _productRepo.GetByIdAsync(orderItem.ProductId);
                if (product == null)
                    throw new Exceptions.NotFoundException($"Product with id {orderItem.ProductId} not found");
                totalprice += orderItem.UnitPriceAtSale * orderItem.Quantity;

                foreach (var extraItem in orderItem.ExtraItems)
                {
                    var isValidExtra = await _productExtraRepo.IsExtraProductRelatedToProduct(orderItem.ProductId, extraItem.ProductId);
                    if (!isValidExtra)
                    {
                        totalprice = 0;
                        throw new Exceptions.NotFoundException($"Extra product {extraItem.ProductId} not related to product {orderItem.ProductId}" );
                    }
                    totalprice += extraItem.UnitPriceAtSale * extraItem.Quantity;
                }}

            var order = _mapper.Map<Order>(makeOrderRequestDTO);
            order.TotalAmountPrice = totalprice;
            order.TotalFee = 10*totalprice/100m;
            order.TotalFeeTotal = (order.TotalFee / 100m + 1) * order.TotalAmountPrice;


            await _unitOfWork.orders.AddAsync(order);
            await _unitOfWork.SaveChangesAsync();

            var orderResponseDTO = _mapper.Map<OrderResponseDTO>(order);
            return orderResponseDTO;
        }


        public async Task<OrderResponseDTO> ConfirmOrderAsync(string orderId)
        {
            var order = await _unitOfWork.orders.GetByIdAsync(orderId);

            if (order == null)
            {
                throw new Exceptions.NotFoundException( $"Order with id {orderId} not found");
            }
            if (order.Status == OrderStatus.Cancelled)
            {
                throw new Exceptions.BadRequestException("Cancelled order cannot be confirmed");
            }
            if (order.Status == OrderStatus.Confirmed)
            {
                throw new Exceptions.BadRequestException("Order is already confirmed");
            }
            order.Status = OrderStatus.Confirmed;

            _unitOfWork.orders.Update(order);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<OrderResponseDTO>(order);
        }

        public async Task<OrderResponseDTO> CancelOrderAsync(string orderId)
        {
            var order = await _unitOfWork.orders.GetByIdAsync(orderId);

            if (order == null)
            {
                throw new Exceptions.NotFoundException($"Order with id {orderId} not found");
            }

            if (order.Status == OrderStatus.Confirmed)
            {
                throw new Exceptions.BadRequestException("Confirmed order cannot be cancelled");
            }
            if (order.Status == OrderStatus.Cancelled)
            {
                throw new Exceptions.BadRequestException("Order is already cancelled");
            }

            order.Status = OrderStatus.Cancelled;

            _unitOfWork.orders.Update(order);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<OrderResponseDTO>(order);
        }

       
        public async Task<OrderResponseDTO> CompleteOrderAsync(string orderId)
        {
            var order = await _unitOfWork.orders.GetByIdAsync(orderId);

            if (order == null)
            {
                throw new Exceptions.NotFoundException($"Order with id {orderId} not found");
            }

            if (order.Status == OrderStatus.Cancelled)
            {
                throw new Exceptions.BadRequestException("Cancelled orders cannot be completed");
            }

            if (order.Status == OrderStatus.Completed)
            {
                throw new Exceptions.BadRequestException("Order is already completed");
            }

            if (order.Status != OrderStatus.Confirmed)
            {
                throw new Exceptions.BadRequestException("Only confirmed orders can be completed");
            }

            order.Status = OrderStatus.Completed;

            _unitOfWork.orders.Update(order);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<OrderResponseDTO>(order);
        }

        public async Task CookOrderAsync(string orderId)
        {
            //get order 
            //check its status should be cofirmed 
            //cook 
            //make recipe of product 
            //inventory update 
            //product order items 
            //change status to completed 

            var order = await _orderRepo.GetOrderWithOrderItemsAndExtraOrderItemsByIdAsync(orderId);
            if (order == null)
            {
                throw new Exceptions.NotFoundException("Order with this id is not found");
            }
            if(order.Status != OrderStatus.Confirmed)
            {
                throw new Exceptions.BadRequestException($"Can not cook order with status {order.Status}");
            }

            foreach (var orderItem in order.OrderItems)
            {
                var recipeItems = await _recipeRepo.GetRecipeByProductIdAsync2(orderItem.ProductId);

                foreach (var recipeItem in recipeItems)
                {
                    var inventory = await _inventoryRepo.GetByIdAsync(recipeItem.InventoryId);
                    //inventory now in memory so i when i update i
                    //change the values in memory and savechanges will reflect the changes in db
                    var required = recipeItem.Quantity * orderItem.Quantity;

                    if (inventory!.CurrentStock < required)
                        throw new Exceptions.BadRequestException($"{inventory.Name} is not enough in stock");

                    inventory.CurrentStock -= required;
                    _unitOfWork.inventories.Update(inventory);
                }

                foreach (var extra in orderItem.Extras)
                {
                    var recipeextraItems = await _recipeRepo.GetRecipeByProductIdAsync2(extra.ProductId);
                    foreach (var recipeItem in recipeextraItems)
                    {
                        var inventory = await _inventoryRepo.GetByIdAsync(recipeItem.InventoryId);
                        var required = recipeItem.Quantity * extra.Quantity;

                        if (inventory!.CurrentStock < required)
                            throw new Exceptions.BadRequestException($"{inventory.Name} is not enough in stock");

                        inventory.CurrentStock -= required;
                        _unitOfWork.inventories.Update(inventory);

                    }
                }
            }

            order.Status = OrderStatus.Completed;

            await _unitOfWork.SaveChangesAsync();


            
        }

    }
}
