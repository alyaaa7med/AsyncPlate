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

        Task<OrderResponseDTO> MakeOrderAsync(MakeOrderRequestDTO requestDTO);
        Task<OrderResponseDTO> ConfirmOrderAsync(string OrderId, string userId);
        Task<OrderResponseDTO> CancelOrderAsync(string OrderId);
        Task CookOrderAsync(string OrderId, string userId);


    }
}
