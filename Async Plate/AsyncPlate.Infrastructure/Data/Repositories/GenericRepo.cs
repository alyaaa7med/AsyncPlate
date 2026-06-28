using AsyncPlate.Application.Interfaces.Repositories;
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

        public void Add(T entity) =>   _context.Set<T>().Add(entity);
        public void AddRange(IEnumerable<T> entities) => _context.AddRange(entities);
        
        public void Update(T entity) => _context.Set<T>().Update(entity);
        public void Delete(T entity) => _context.Set<T>().Remove(entity);
        public void DeleteRange(IEnumerable<T> entities) => _context.RemoveRange(entities);

        public IQueryable<T> GetAll() => _context.Set<T>().AsQueryable();

    }
}
