using AsyncPlate.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AsyncPlate.Application.Interfaces.Repositories
{
    public interface ICategoryRepo:IBaseRepo<Category>
    {
        Task<bool> AnyCategoryAsync(string name);
        Task<Category?> GetCategoryWithRelatedDataAsync(string categoryId);
        Task<List<Category>> GetCategoriesByIdsAsync(List<string> categoryIds);
    }
}
