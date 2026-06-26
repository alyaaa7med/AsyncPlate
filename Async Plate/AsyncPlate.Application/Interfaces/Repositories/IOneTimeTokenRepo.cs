using AsyncPlate.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Application.Interfaces.Repositories
{
    public interface IOneTimeTokenRepo : IBaseRepo<OneTimeToken>
    {


        //bad for db: Task<OneTimeToken?> GetActiveOneTimeTokenAsync(Func<OneTimeToken, bool> predicate);//fun(onetimetoken)
        //return bool 

        Task<OneTimeToken?> GetActiveOneTimeTokenAsync(Expression<Func<OneTimeToken, bool>> predicate);

        Task<List<OneTimeToken>> GetAllTokenByUserIdAsync(string userId);
        void RemoveRange(IEnumerable<OneTimeToken> tokens);

    }
}
