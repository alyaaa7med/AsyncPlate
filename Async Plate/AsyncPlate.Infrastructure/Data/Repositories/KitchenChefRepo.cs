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
    public class KitchenChefRepo : GenericRepo<KitchenChef>,IKitchenChefRepo
    {
        public KitchenChefRepo(AppDbContext context) : base(context)
        {
            // create / update / get / delete are in the generic repo no need to re-implement them here
        }
      
        
    

        public async Task<List<string>> GetChefUserIdsAsync()
        {
            return await _context.Chefs
                .AsNoTracking()
                .Select(c => c.AppUserId)
                .ToListAsync();
        }


        public IQueryable<KitchenChef> GetAllWithUsers()
        {
            return _context.Chefs
                .AsNoTracking()
                .Include(c => c.AppUser);
        }

        public async Task<KitchenChef?> GetWithUserByUserIdAsync(string userId)
        {
            return await _context.Chefs
                .Include(c => c.AppUser)
                .FirstOrDefaultAsync(c => c.AppUserId == userId);
        }



    }
}
