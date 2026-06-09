using AsyncPlate.Application.Interfaces.Repositories;
using AsyncPlate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Infrastructure.Data.Repositories
{
    public class OrderItemRepo : GenericRepo<OrderItem> ,IOrderItemRepo
    {
        public OrderItemRepo(AppDbContext context) : base(context)
        {
        }
    
    }
}
