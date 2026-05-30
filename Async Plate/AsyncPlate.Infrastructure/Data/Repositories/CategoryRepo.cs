using AsyncPlate.Core.Entities;
using AsyncPlate.Core.Interfaces;
using AsyncPlate.Core.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Infrastructure.Data.Repositories
{
    public class CategoryRepo :GenericRepo<Category>, ICategoryRepo
    {
        public CategoryRepo(AppDbContext context) : base(context)
        {
        }

        public async Task<bool> AnyCategoryAsync(string name)
        {
            return await _context.Categories.AnyAsync(c => c.Name.ToLower() == name.ToLower());
        }

        public async Task<Category?> GetCategoryWithRelatedDataAsync(string categoryId)
        {
            return await _context.Categories
                .Include(c => c.CurrentOffer)
                .Include(c=> c.Products)
                .SingleOrDefaultAsync(c => c.Id == categoryId);
        }
    }
}
