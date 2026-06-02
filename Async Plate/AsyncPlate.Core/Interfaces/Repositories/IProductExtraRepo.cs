using AsyncPlate.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Core.Interfaces.Repositories
{
    public interface IProductExtraRepo : IBaseRepo<ProductExtra>
    {
        Task<bool> IsExtraProductRelatedToProduct(string productId, string extraProductId);
    }
}
