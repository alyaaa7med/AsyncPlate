using AsyncPlate.Core.DTOs.Order;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Core.Services.Interfaces
{
    public interface IOrderService
    {
        //make order
        //confirm order 
        //cancel order
        //cook up order

        Task<OrderResponseDTO> MakeOrderAsync(MakeOrderRequestDTO requestDTO);
        Task<OrderResponseDTO> ConfirmOrderAsync(string OrderId);
        Task<OrderResponseDTO> CancelOrderAsync(string OrderId);
        Task<OrderResponseDTO> CompleteOrderAsync(string orderId);
        Task CookOrderAsync(string OrderId);



    }
}
