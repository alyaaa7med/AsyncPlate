using AsyncPlate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Application.Interfaces.Repositories
{
    public interface IOrderRepo:IBaseRepo<Order>
    {
        Task<Order?> GetOrderWithOrderItemsAndExtraOrderItemsByIdAsync(string orderId);
            

    }
}
