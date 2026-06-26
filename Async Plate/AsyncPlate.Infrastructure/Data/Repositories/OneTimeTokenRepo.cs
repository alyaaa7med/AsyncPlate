using AsyncPlate.Application.Interfaces.Repositories;
using AsyncPlate.Domain.Entities;
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
                .SingleOrDefaultAsync(predicate);
        }


        public async Task<List<OneTimeToken>> GetAllTokenByUserIdAsync(string userId)
        {
            return await _context.OneTimeToken.Where(o=>o.AppUserId == userId).ToListAsync();
                
        }
        public void RemoveRange(IEnumerable<OneTimeToken> tokens)
        {
             _context.OneTimeToken.RemoveRange(tokens);

        }
    }
}


