using AsyncPlate.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Core.Interfaces.Services
{
    public  interface ITokenService
    {
        string CreateAccessToken(AppUser user);
        string GenerateRefreshToken();
    }
}
