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
    public class RefreshTokenRepo : GenericRepo<RefreshToken>,IRefreshTokenRepo
    {
        public RefreshTokenRepo(AppDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<RefreshToken>> FindActiveTokensByUserIdAsync(string userId)//i only need enumerating (abstraction of list) no need for list specifications 
        {
            return await _context.RefreshTokens.Where(t => t.UserId == userId && !t.IsRevoked && !t.IsExpired).ToListAsync();
        }


        //find specife refresh token by user id and token value  

        public async Task<RefreshToken?> CheckRefreshTokenAsync( string tokenValue)
        {
            return await _context.RefreshTokens.FirstOrDefaultAsync(t =>  t.RefreshTokenValue == tokenValue && !t.IsRevoked && !t.IsExpired);
        }

        public async Task InvalidateRefreshTokensByUserIdAsync(string userId)
        {
            var refreshTokens = await _context.RefreshTokens.Where(t => t.UserId == userId).ToListAsync();
            foreach (var token in refreshTokens)
            {
                token.IsRevoked = true;
                token.IsExpired = true;
                token.ExpiredAt = DateTime.UtcNow;
            }
        }
      

    }
}
