using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Application.Jobs
{
    public interface IInventoryJob
    {
        Task SendLowStockInventoryNotification(string inventoryId);
        Task SendLowStockSuppliersEmail();

    }
}
