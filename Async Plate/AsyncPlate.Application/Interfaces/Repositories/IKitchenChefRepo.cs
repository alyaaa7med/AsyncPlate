using AsyncPlate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Application.Interfaces.Repositories
{
    public interface IKitchenChefRepo : IBaseRepo<KitchenChef>
    {
        Task<List<string>> GetChefUserIdsAsync();
        IQueryable<KitchenChef> GetAllWithUsers();
        Task<KitchenChef?> GetWithUserByUserIdAsync(string userId);


    }
}
