using AsyncPlate.Core.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Infrastructure.Data.Repositories
{
    public class CustomerRepo : GenericRepo<Core.Entities.Customer>, ICustomerRepo
    {
        public CustomerRepo(AppDbContext context) : base(context)
        {
            // create / update / get / delete are in the generic repo no need to re-implement them here
        }
    }
}
