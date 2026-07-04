using AsyncPlate.Application.DTOs.Order;
using AsyncPlate.Application.Interfaces;
using AsyncPlate.Application.Interfaces.Jobs;
using AsyncPlate.Application.Interfaces.Repositories;
using AsyncPlate.Application.Services.Interfaces;
using AsyncPlate.Domain.Entities;
using AutoMapper;
using FluentValidation;
using Hangfire;
using Microsoft.Extensions.Logging;

namespace AsyncPlate.Application.Services.Implementation
{
    public class OrderService : IOrderService
    {
        private readonly ILogger<OrderService> _logger;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IValidator<MakeOrderRequestDTO> _validator1;
        private readonly ICustomerRepo _customerRepo;
        private readonly IKitchenChefRepo _chefRepo;
        private readonly IProductRepo _productRepo;
        private readonly IRecipeRepo _recipeRepo;
        private readonly IProductExtraRepo _productExtraRepo;
        private readonly IOrderRepo _orderRepo;
        private readonly IOrderItemRepo _orderItemRepo;
        private readonly IOrderExtraItemRepo _orderExtraItemRepo;
        private readonly IInventoryRepo _inventoryRepo;
        private readonly IOrderJob _orderJob;
        public OrderService(ILogger<OrderService> logger, IMapper mapper, IUnitOfWork unitOfWork,
            IValidator<MakeOrderRequestDTO> validator1, ICustomerRepo customerRepo,IKitchenChefRepo chefRepo
            , IProductRepo productRepo, IProductExtraRepo productExtraRepo, IRecipeRepo recipeRepo
            , IOrderRepo orderRepo, IOrderItemRepo orderItemRepo, IOrderExtraItemRepo orderExtraItemRepo, IInventoryRepo inventoryRepo,
            IOrderJob orderJob
            )
        {

            _logger = logger;
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _validator1 = validator1;
            //_validator2 = validator2; no need for this as faluent validation will automatically resolve the validator for OrderItemRequestDTO when validating the MakeOrderRequestDTO which contains a list of OrderItemRequestDTO
            _customerRepo = customerRepo;
            _chefRepo = chefRepo;
            _productRepo = productRepo;
            _recipeRepo = recipeRepo;
            _productExtraRepo = productExtraRepo;
            _orderRepo = orderRepo;
            _orderItemRepo = orderItemRepo;
            _orderExtraItemRepo = orderExtraItemRepo;
            _inventoryRepo = inventoryRepo;
            _orderJob = orderJob;
        }
        public async Task<OrderResponseDTO> MakeOrderAsync(MakeOrderRequestDTO makeOrderRequestDTO)
        {
            //check userid from jwt and get customerid from it and set it in the dto 
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
                _logger.LogWarning("Validation failed for MakeOrder. Errors: {@Errors}", errorsDictionary);
                throw new Exceptions.ValidationException(errorsDictionary);
            }

            decimal totalprice = 0;

            foreach (var orderItem in makeOrderRequestDTO.OrderItems)
            {
                _logger.LogInformation("Processing ProductId: {ProductId}", orderItem.ProductId);
                var product = await _productRepo.GetByIdAsync(orderItem.ProductId);
                if (product == null)
                {
                    _logger.LogWarning("Product not found. ProductId: {ProductId}", orderItem.ProductId);
                    throw new Exceptions.NotFoundException($"Product with id {orderItem.ProductId} not found");
                }
                product.TotalTimesOrdered+= orderItem.Quantity;
                totalprice += orderItem.UnitPriceAtSale * orderItem.Quantity;

                foreach (var extraItem in orderItem.ExtraItems)
                {
                    var isValidExtra = await _productExtraRepo.IsExtraProductRelatedToProduct(orderItem.ProductId, extraItem.ProductId);
                    if (!isValidExtra)
                    {
                        totalprice = 0;
                        throw new Exceptions.NotFoundException($"Extra product {extraItem.ProductId} not related to product {orderItem.ProductId}" );
                    }
                    var extraProduct = await _productRepo.GetByIdAsync(extraItem.ProductId);
                    extraProduct!.TotalTimesOrdered += extraItem.Quantity;
                    totalprice += extraItem.UnitPriceAtSale * extraItem.Quantity;
                }}

            var order = _mapper.Map<Order>(makeOrderRequestDTO);
            order.TotalAmountPrice = totalprice;
            order.TotalFee = 10*totalprice/100m;
            order.TotalFeeTotal = (order.TotalFee / 100m + 1) * order.TotalAmountPrice;


             _unitOfWork.orders.Add(order);
            await _unitOfWork.SaveChangesAsync();

            _logger.LogInformation("Order created successfully. OrderId: {OrderId}, Total: {Total}", order.Id,order.TotalAmountPrice);
            var orderResponseDTO = _mapper.Map<OrderResponseDTO>(order);
            return orderResponseDTO;
        }

        public async Task<OrderResponseDTO> ConfirmOrderAsync(string orderId, string userId)
        {
            var customer = await _unitOfWork.customers.GetWithUserByUserIdAsync(userId);
            if (customer == null)
            {
                _logger.LogWarning("Customer not found. UserId: {UserId}", userId);
                throw new Exceptions.NotFoundException($"Customer with user id {userId} not found");
            }
          
            var order = await _unitOfWork.orders.GetByIdAsync(orderId);

            if (order == null)
            {
                _logger.LogWarning("order not found. OrderId: {OrderId}", orderId);
                throw new Exceptions.NotFoundException( $"Order with id {orderId} not found");
            }
            if (order.Status == OrderStatus.Cancelled)
            {
                _logger.LogWarning("Cannot confirm cancelled order. OrderId: {OrderId}", orderId);
                throw new Exceptions.BadRequestException("Cancelled order cannot be confirmed");
            }
            if (order.Status == OrderStatus.Confirmed)
            {
                _logger.LogWarning("Cannot confirm confirmed order. OrderId: {OrderId}", orderId);
                throw new Exceptions.BadRequestException("Order is already confirmed");
            }

          
            order.Status = OrderStatus.Confirmed;
            order.Customer = customer;
            customer.LoyaltyPoints += (int)order.TotalAmountPrice; // 1 point for every 1 currency units spent
            //no need to update customer/order as it is tracked by getting them

            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("Order confirmed successfully. OrderId: {OrderId}", orderId);

            //we need to send realtime notifaction to the chefs to cook => background job 
            BackgroundJob.Enqueue<IOrderJob>(job => job.SendNewOrderNotificationAsync(order.Id));


            return _mapper.Map<OrderResponseDTO>(order);

        }

        public async Task<OrderResponseDTO> CancelOrderAsync(string orderId)
        {
            var order = await _unitOfWork.orders.GetByIdAsync(orderId);

            if (order == null)
            {
                _logger.LogWarning("Order not found. OrderId: {OrderId}", orderId);
                throw new Exceptions.NotFoundException($"Order with id {orderId} not found");
            }

            if (order.Status == OrderStatus.Confirmed)
            {
                _logger.LogWarning("Cannot cancel confirmed order. OrderId: {OrderId}", orderId);
                throw new Exceptions.BadRequestException("Confirmed order cannot be cancelled");
            }
            if (order.Status == OrderStatus.Cancelled)
            {
                _logger.LogWarning("Order already cancelled. OrderId: {OrderId}", orderId);
                throw new Exceptions.BadRequestException("Order is already cancelled");
            }

            order.Status = OrderStatus.Cancelled;

            await _unitOfWork.SaveChangesAsync();
            _logger.LogInformation("Order cancelled successfully. OrderId: {OrderId}", orderId);


            return _mapper.Map<OrderResponseDTO>(order);
        }

        public async Task CookOrderAsync(string orderId, string userId)
        {

            var order = await _unitOfWork.orders.GetOrderWithOrderItemsAndExtraOrderItemsByIdAsync(orderId);

            if (order == null)
            {
                throw new Exceptions.NotFoundException("Order not found");
            }

            if (order.Status != OrderStatus.Confirmed)
            {
                throw new Exceptions.BadRequestException($"Cannot cook order with status {order.Status}");
            }

            var chef = await _unitOfWork.kitchenChefs.GetWithUserByUserIdAsync(userId);
            order.KitchenChef = chef;

            //get all products main and extra and remove duplicates
            var productIds = order.OrderItems .Select(oi => oi.ProductId)
                .Concat(order.OrderItems.SelectMany(oi => oi.Extras).Select(e => e.ProductId)).Distinct().ToList();

            //load all recipes in 1 query for all products to avoid calling in loop (n+1 problem)
            var recipes = await _unitOfWork.recipes.GetRecipesByProductIdsAsync(productIds);

            //select all inventory ids and remove duplicates 
            var inventoryIds = recipes.Select(r => r.InventoryId).Distinct().ToList();

            //load all inventories in 1 query to avoid n+1 
            var inventories = await _unitOfWork.inventories.GetInventoriesByIdsAsync(inventoryIds);

           //dictiories for o(1) lookup (= search)

            var recipesByProduct = recipes.GroupBy(r => r.ProductId).ToDictionary( g => g.Key,g => g.ToList());
            //{"pizza proudct id", Recipe ["tomato sauce", "cheese"]}
            var inventoriesById = inventories.ToDictionary(i => i.Id);
            //{"tomato sauce id", Inventory [Id = "tomato sauce id",  CurrentStock = 100 ]}

            //for background low stock inventories
            var lowStockInventories = new HashSet<string>();

            foreach (var orderItem in order.OrderItems)
            {
                if (recipesByProduct.TryGetValue(orderItem.ProductId, out var recipeItems))
                {
                    foreach (var recipeItem in recipeItems)
                    {
                        var inventory =inventoriesById[recipeItem.InventoryId];
                        var required = recipeItem.Quantity * orderItem.Quantity;
                        if (inventory.CurrentStock < required)
                        {
                            throw new Exceptions.BadRequestException( $"{inventory.Name} is not enough in stock");
                        }
                        inventory.CurrentStock -= required;
                        //product = not available => manually by the admin 

                        //no need to update as the inventory is tracked already in memory and the update is reflected in memory 

                        if (inventory.CurrentStock < inventory.MinStockLevel)
                        {
                            lowStockInventories.Add(inventory.Id);
                        }
                    }
                }
                //extras

                foreach (var extra in orderItem.Extras)
                {
                    if (recipesByProduct.TryGetValue(extra.ProductId, out var extraRecipeItems))
                    {
                        foreach (var recipeItem in extraRecipeItems)
                        {
                            var inventory = inventoriesById[recipeItem.InventoryId];
                            var required = recipeItem.Quantity * extra.Quantity;

                            if (inventory.CurrentStock < required)
                            {
                                throw new Exceptions.BadRequestException($"{inventory.Name} is not enough in stock");
                            }
                            inventory.CurrentStock -= required;
                            //no need to update as the inventory is tracked already in memory and the update is reflected in memory 

                            if (inventory.CurrentStock < inventory.MinStockLevel)
                            {
                                lowStockInventories.Add(inventory.Id);
                            }
                        }
                    }
                }
                //no need for transaction as if the extra failed the updated quantity will be still in memory but for that order only
                //no affect the other orders
            }

            order.Status = OrderStatus.Cooking;

            await _unitOfWork.SaveChangesAsync();

            //send notification to customer that order is cooking by a chef
            BackgroundJob.Enqueue<IOrderJob>(job => job.SendCookingOrderNotificationAsync(order.Id));

            //send notification to admin and chef for low stock inventory 
            foreach (var inventoryid in lowStockInventories) {
                BackgroundJob.Enqueue<IInventoryJob>(job => job.SendLowStockInventoryNotification(inventoryid));
            }



        }

        /*still N+1 problem 
        by getting all the data we need in one query and then do the operations in memory without making multiple calls to the database which is costly in terms of performance
        public async Task CookOrderAsync(string orderId)
        {
            var order = await _orderRepo.GetOrderWithOrderItemsAndExtraOrderItemsByIdAsync(orderId);
            if (order == null)
            {
                throw new Exceptions.NotFoundException("Order with this id is not found");
            }
            if(order.Status != OrderStatus.Confirmed)
            {
                throw new Exceptions.BadRequestException($"Can not cook order with status {order.Status}");
            }
            //inventories for main and extra 
            var inventoryIds = order.OrderItems
                .SelectMany(oi => _recipeRepo.GetRecipeByProductIdAsync2(oi.ProductId).Result.Select(ri => ri.InventoryId))
                .Concat(order.OrderItems.SelectMany(oi => oi.Extras)
                    .SelectMany(e => _recipeRepo.GetRecipeByProductIdAsync2(e.ProductId).Result.Select(ri => ri.InventoryId)))
                .Distinct().ToList();

            var inventories = await _inventoryRepo.GetInventoriesByIdsAsync(inventoryIds);
            var inventoryDict = inventories.ToDictionary(i => i.Id);

            foreach (var orderItem in order.OrderItems)
            {
                var recipeItems = await _recipeRepo.GetRecipeByProductIdAsync2(orderItem.ProductId);
                foreach (var recipeItem in recipeItems)
                {
                    var inventory = inventoryDict[recipeItem.InventoryId];
                    var required = recipeItem.Quantity * orderItem.Quantity;
                    if (inventory.CurrentStock < required)
                        throw new Exceptions.BadRequestException($"{inventory.Name} is not enough in stock");
                    inventory.CurrentStock -= required;
                    _unitOfWork.inventories.Update(inventory);
                }
                foreach (var extra in orderItem.Extras)
                {
                    var recipeextraItems = await _recipeRepo.GetRecipeByProductIdAsync2(extra.ProductId);
                    foreach (var recipeItem in recipeextraItems)
                    {
                        var inventory = inventoryDict[recipeItem.InventoryId];
                        var required = recipeItem.Quantity * extra.Quantity;
                        if (inventory.CurrentStock < required)
                            throw new Exceptions.BadRequestException($"{inventory.Name} is not enough in stock");
                        inventory.CurrentStock -= required;
                        _unitOfWork.inventories.Update(inventory);
                    }
                }
            }
            order.Status = OrderStatus.Completed;
            await _unitOfWork.SaveChangesAsync();

        }
        */

        /* N+ 1 problem
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
        */

        public async Task CompleteOrderAsync(string orderId)
        {
            //check if the chef is the assgined chef for that order 

            var order = await _unitOfWork.orders.GetByIdAsync(orderId);
            if (order == null)
            {
                throw new Exceptions.NotFoundException("Order not found");

            }
            order.Status = OrderStatus.Completed;
            await _unitOfWork.SaveChangesAsync();

            BackgroundJob.Enqueue<IOrderJob>(job => job.SendCompleteOrderNotificationAsync(order.Id));

        }
    }
}
