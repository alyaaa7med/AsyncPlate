using AsyncPlate.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Application.Interfaces.Repositories
{
    public interface ICustomerRepo : IBaseRepo<Customer>
    {
        Task<Customer?> GetWithUserByUserIdAsync(string userId);
        Task<List<string>> GetVipCustomerUserIdsAsync();
        IQueryable<Customer> GetVipCustomers();
        IQueryable<Customer> GetAllWithUsers();


    }
}
