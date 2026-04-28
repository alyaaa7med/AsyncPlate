using AsyncPlate.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Infrastructure.Repository
{
    public class GuestRepo : GenericRepo<Core.Entities.Customer>, ICustomerRepo
    {
        public GuestRepo(AppDbContext context) : base(context)
        {
        }
    }
}
