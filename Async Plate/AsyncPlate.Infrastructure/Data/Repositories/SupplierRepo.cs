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
    public class SupplierRepo : GenericRepo<Supplier>, ISupplierRepo
    {
        public SupplierRepo(AppDbContext context) : base(context)
        {

        }

        public async Task<bool> AnySupplierAsync(string email)
        {
            return await _context.Suppliers.AnyAsync(s => s.ContactEmail == email);
        }

        public IQueryable<Supplier> FilterByName(string name)//not async as it just build the query not execute it 
        {
            return _context.Suppliers.Where(x => x.Name.Contains(name));
        }
    }
}
