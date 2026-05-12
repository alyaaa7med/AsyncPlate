using AsyncPlate.Core.Entities;
using AsyncPlate.Core.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Infrastructure.Data.Repositories
{
    public class KitchenChefRepo : GenericRepo<KitchenChef>,IKitchenChefRepo
    {
        public KitchenChefRepo(AppDbContext context) : base(context)
        {
            // create / update / get / delete are in the generic repo no need to re-implement them here
        }
    }
}
