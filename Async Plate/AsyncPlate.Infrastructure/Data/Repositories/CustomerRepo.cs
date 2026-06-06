using AsyncPlate.Core.Entities;
using AsyncPlate.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
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

        //get by userid
        public async Task<Customer?> GetByUserIdAsync(string userId)
        {
            return await _context.Customers.FirstOrDefaultAsync(c => c.AppUserId == userId);
        }

        public async Task<IEnumerable<string>> GetVipCustomerIdsAsync()
        {
            return await _context.Customers.Where(u => u.LoyaltyPoints > 1220)
                                           .Select(u => u.Id)
                                           .ToListAsync();
        }
    }

}
