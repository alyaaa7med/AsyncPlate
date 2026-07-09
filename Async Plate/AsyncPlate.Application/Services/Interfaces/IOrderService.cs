using AsyncPlate.Application.Common.DTOs;
using AsyncPlate.Application.DTOs.Offer;
using AsyncPlate.Application.DTOs.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Application.Services.Interfaces
{
    public interface IOrderService
    {
        //make order
        //confirm order 
        //cancel order
        //cook up order
        //get confrimed orders= not cooked yet

        Task<OrderResponseDTO> MakeOrderAsync(string userId, MakeOrderRequestDTO requestDTO);
        Task<OrderResponseDTO> ConfirmOrderAsync(string OrderId, string userId);
        Task<OrderResponseDTO> CancelOrderAsync(string OrderId, string userId);
        Task CookOrderAsync(string OrderId, string userId);
        Task<OrderResponseDTO> CompleteOrderAsync(string OrderId, string userId);
        Task<IEnumerable<OrderResponseDTO>> GetChefActiveOrdersAsync(string userId); //confirmed
                                                                                     //Task<IEnumerable<OrderResponseDTO>> GetChefCompletedOrdersAsync(string userId);//comleted => in the future
        Task<OrderResponseDTO> GetOrderByIdAsync(string orderId);
        Task<PagedResult<OrderResponseDTO>> GetAllOrdersWithRelatedDataAsync(int pageNumber, int pageSize);



    }
}
