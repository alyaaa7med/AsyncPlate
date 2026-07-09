using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AsyncPlate.Domain.Entities;

namespace AsyncPlate.Application.DTOs.Order
{
    public class OrderStatusChangeDTO
    {
        public string OrderId { get; set; } = string.Empty;
        public string Status { get; set; } = string.Empty;

    }
}
