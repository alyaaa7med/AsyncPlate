using AsyncPlate.Application.Interfaces.Repositories;
using AsyncPlate.Domain.Entities;
using Microsoft.EntityFrameworkCore;

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
            //eager loading ...
            return await _context.Categories
                .Include(c => c.CurrentOffer)
                .Include(c=> c.Products)
                .SingleOrDefaultAsync(c => c.Id == categoryId);
        }

     
        public IQueryable<Category> GetAllWithRelatedData()
        {
            //eager loading ...
            return _context.Categories
                .Include(c => c.CurrentOffer)
                .Include(c=> c.Products)
                .AsNoTracking();
        }
        public async Task<List<Category>> GetCategoriesByIdsAsync(List<string> categoryIds)
        {
            return await _context.Categories.Where(c => categoryIds.Contains(c.Id)).ToListAsync();
        }
    }
}
