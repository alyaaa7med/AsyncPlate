using AsyncPlate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Application.Interfaces.Services
{
    public  interface ITokenService
    {
        string CreateAccessToken(AppUser user);
        string GenerateRefreshToken();
    }
}
