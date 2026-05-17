using AsyncPlate.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Core.Interfaces.Repositories
{
    public interface IRefreshTokenRepo : IBaseRepo<RefreshToken>
    {

        Task<IEnumerable<RefreshToken>> FindActiveTokensByUserIdAsync(string userId);//the tokens all aready in memory but enumerated to remove list features 
        Task<RefreshToken?> CheckRefreshTokenAsync(string tokenValue);

        //remove refreshtokens to logout from all devices 
        Task InvalidateRefreshTokensByUserIdAsync(string userId);

    }
}
