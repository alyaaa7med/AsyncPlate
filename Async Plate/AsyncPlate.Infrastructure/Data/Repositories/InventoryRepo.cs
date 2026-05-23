using AsyncPlate.Core.Entities;
using AsyncPlate.Core.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Infrastructure.Data.Repositories
{
    public class InventoryRepo : GenericRepo<Inventory>, IInventoryRepo
    {
        public InventoryRepo(AppDbContext context) : base(context)
        {
        }
    }
}
