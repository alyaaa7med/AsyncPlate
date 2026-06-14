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
    public class AdminRepo : GenericRepo<Admin>, IAdminRepo
    {
        public AdminRepo(AppDbContext context) : base(context)
        {
            // create / update / get / delete are in the generic repo no need to re-implement them here
        }
        public async Task<List<string>> GetAdminUserIdsAsync()
        {
            return await _context.Admins
                //.AsNoTracking()
                .Select(c => c.AppUserId)
                .ToListAsync();
        }

        public async Task<List<string?>> GetAdminsEmailsAsync()
        {
            return await _context.Admins
                .Include(a=>a.AppUser)
                .Select(a => a.AppUser.Email)
                .ToListAsync();
        }
    }
}
