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
    public class UserRepo : GenericRepo<AppUser>, IUserRepo
    {
        public UserRepo(AppDbContext context) : base(context)
        {

        }

        public Task<AppUser?> GetByEmailAsync(string email)
        {
            return _context.Users.FirstOrDefaultAsync(u => u.Email == email);//for reference type the default is null
        }
        
    }
}
