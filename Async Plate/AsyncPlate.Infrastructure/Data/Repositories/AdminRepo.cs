using AsyncPlate.Application.Interfaces.Repositories;
using AsyncPlate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Infrastructure.Data.Repositories
{
    public class AdminRepo : GenericRepo<Admin>, IAdminRepo
    {
        public AdminRepo(AppDbContext context) : base(context)
        {
            // create / update / get / delete are in the generic repo no need to re-implement them here
        }
    }
}
