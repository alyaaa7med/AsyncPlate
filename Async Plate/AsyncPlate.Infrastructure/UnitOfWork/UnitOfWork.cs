using AsyncPlate.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {

        private readonly AppDbContext _context;

        //i need to define all repositories here and then will add/update/..  
        //note : i will use the interface of the unitofwork in the service so i need it to have the properties and the abstract methods
        

        public UnitOfWork(AppDbContext context)
        {
            _context = context;

        }

        public async Task<int> SaveChangesAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}