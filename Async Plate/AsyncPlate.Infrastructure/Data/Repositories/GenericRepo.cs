using AsyncPlate.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Infrastructure.Data.Repositories
{
    public class GenericRepo<T> : IBaseRepo<T> where T : class
    {

        //base for all entities + will deal with dbcontext 
        protected readonly AppDbContext _context;
        public GenericRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<T?> GetByIdAsync(string id) => await _context.Set<T>().FindAsync(id); // returns entity or null
        public IQueryable<T> GetAll() => _context.Set<T>().AsQueryable();
        public async Task AddAsync(T entity) => await _context.Set<T>().AddAsync(entity);


        public void Update(T entity) => _context.Set<T>().Update(entity);
        public void Delete(T entity) => _context.Set<T>().Remove(entity);

    }
}
