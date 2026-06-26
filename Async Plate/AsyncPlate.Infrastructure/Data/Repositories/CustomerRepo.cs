using AsyncPlate.Application.Interfaces.Repositories;
using AsyncPlate.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Infrastructure.Data.Repositories
{
    public class CustomerRepo : GenericRepo<Customer>, ICustomerRepo
    {
        public CustomerRepo(AppDbContext context) : base(context)
        {
            // create / update / get / delete are in the generic repo no need to re-implement them here
        }

        //get by userid
      

        public async Task<Customer?> GetWithUserByUserIdAsync(string userId)
        {
            return await _context.Customers.Include(c=>c.AppUser).
                FirstOrDefaultAsync(c => c.AppUserId == userId);

        }

        public async Task<List<string>> GetVipCustomerUserIdsAsync()
        {
            return await _context.Customers
                .AsNoTracking()
                .Where(c => c.LoyaltyPoints > 1220)
                .Select(c => c.AppUserId) 
                .ToListAsync();
        }

        public IQueryable<Customer> GetVipCustomers()
        {
            return _context.Customers
                .AsNoTracking()
                .Where(c => c.LoyaltyPoints>1500)
                .Include(c => c.AppUser);
        }

        public IQueryable<Customer> GetAllWithUsers()
        {
            return _context.Customers
                .AsNoTracking()
                .Include(c => c.AppUser);
        }
    }

}
