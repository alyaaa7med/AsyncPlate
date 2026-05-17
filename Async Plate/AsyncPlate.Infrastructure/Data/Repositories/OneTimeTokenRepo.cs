using AsyncPlate.Core.Entities;
using AsyncPlate.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Infrastructure.Data.Repositories
{
    public class OneTimeTokenRepo : GenericRepo<OneTimeToken>, IOneTimeTokenRepo
    {
        public OneTimeTokenRepo(AppDbContext context) : base(context)
        {
        }

       
        public async Task<OneTimeToken?> GetActiveOneTimeTokenAsync(Expression<Func<OneTimeToken, bool>> predicate)
        {
            return await _context.Set<OneTimeToken>()
                .FirstOrDefaultAsync(predicate);
        }
    }
}


